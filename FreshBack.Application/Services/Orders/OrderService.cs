using AutoMapper;
using FreshBack.Application.Dtos.Orders;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Orders;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Common.Extensions;
using FreshBack.Domain.Interfaces.Repositories.Orders;
using FreshBack.Domain.Interfaces.Repositories.Products;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Orders;
using FreshBack.Domain.Models.Products;
using FreshBack.Domain.Models.ProductsOrders;
using FreshBack.Domain.Models.Shared;
using FreshBack.Domain.Specifications.Absraction;
using FreshBack.Domain.Specifications.Orders;

namespace FreshBack.Application.Services.Orders;

public class OrderService(
    IOrderRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IProductRepository productRepository) : BaseService<
    CreateOrderDto,
    OrderDto,
    OrderDto,
    OrderDto,
    Order,
    int>(repository, unitOfWork, mapper), IOrderService
{
    private readonly IOrderRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<ResultDto<CreateOrderDto>> CreateAsync(
        CreateOrderDto createOrderDto,
        int customerId)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Order",
            action: async () =>
            {
                var orderProducts = createOrderDto.ProductsOrders!
                    .ToDictionary(x => x.ProductId, x => x.Quantity);
                var productIds = orderProducts.Keys.ToList();
                var spec = new BaseSpecification<Product>
                {
                    Criteria = p => productIds.Contains(p.Id)
                };
                var products = await _productRepository.GetAllAsync(spec);

                if (products.Count() != productIds.Count)
                    throw new Exception("One or more products do not exist.");

                var merchantId = products.First().MerchantId;
                var differentMerchantProducts = products
                    .Where(p => p.MerchantId != merchantId)
                    .ToList();

                if (differentMerchantProducts.Count != 0)
                    throw new Exception(
                        "All products in an order must belong to the same merchant."
                    );

                var insufficientProducts = products
                    .Where(p => p.Quantity < orderProducts[p.Id])
                    .ToList();

                if (insufficientProducts.Count != 0)
                {
                    var message = string.Join(" | ",
                        insufficientProducts.Select(p =>
                            $"Product '{p.Name}' has {p.Quantity} available, " +
                            $"but {orderProducts[p.Id]} was requested."
                        ));

                    throw new Exception(message);
                }

                var order = _mapper.Map<Order>(createOrderDto);

                order.CustomerId = customerId;

                await _repository.CreateAsync(order);

                var orderCreated = await _unitOfWork.Complete();

                if (!orderCreated)
                    throw new Exception("Failed to create order");

                return _mapper.Map<CreateOrderDto>(createOrderDto);
            });
    }

    public async override Task<ResultDto<OrderDto>> GetAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Order",
            action: async () =>
            {
                var spec = new BaseSpecification<Order>
                {
                    IncludeChains =
                    [
                        new IncludeChain<Order>
                        {
                            InitialInclude = o => o.ProductsOrders,
                            ThenIncludes = [
                                po => (po as ProductOrder)!.Product
                            ]
                        }
                    ]
                };

                var order = await _repository.GetAsync(id, spec);

                return _mapper.Map<OrderDto>(order);
            });
    }

    public async override Task<ResultDto<PagedResult<OrderDto>>> GetAllPaginatedAsync(
        PaginatedModelDto paginatedModelDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Order",
            action: async () =>
            {
                var spec = new BaseSpecification<Order>
                {
                    Includes =
                    [
                        o => o.Merchant
                    ]
                };

                var (orders, totalCount) = await _repository.GetAllPaginatedAsync(
                    _mapper.Map<PaginatedModel>(paginatedModelDto), spec);

                return new PagedResult<OrderDto>(
                    _mapper.Map<IReadOnlyList<OrderDto>>(orders), totalCount);
            });
    }

    public async Task<ResultDto<PagedResult<OrderDto>>> GetCustomerOrders(
        GetCustomerPreviousOrdersDto getCustomerPreviousOrdersDto,
        int customerId)
    {
        return await ExecuteServiceCallAsync(
            "Get All Branches Paginated",
            async () =>
            {
                var spec = new OrdersForCustomerSpecification(
                    customerId,
                    getCustomerPreviousOrdersDto.Status,
                    getCustomerPreviousOrdersDto.SortBy,
                    getCustomerPreviousOrdersDto.SortDirection);
                var paginatedModel =
                    _mapper.Map<PaginatedModel>(getCustomerPreviousOrdersDto);

                var (orders, totalCount) =
                    await _repository.GetAllPaginatedAsync(
                        paginatedModel,
                        spec);

                return new PagedResult<OrderDto>(
                    _mapper.Map<IReadOnlyList<OrderDto>>(orders),
                    totalCount);
            });
    }
}
