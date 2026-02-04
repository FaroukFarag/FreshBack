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
                var orderProducts = BuildOrderProductsDictionary(createOrderDto);
                var products = await GetAndValidateProducts(orderProducts.Keys);

                ValidateSameMerchant(products);
                ValidateProductAvailability(products, orderProducts, createOrderDto.BranchId);

                var order = await CreateOrder(createOrderDto, customerId);

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

    private Dictionary<int, int> BuildOrderProductsDictionary(CreateOrderDto dto)
    {
        return dto.ProductsOrders!.ToDictionary(x => x.ProductId, x => x.Quantity);
    }

    private async Task<List<Product>> GetAndValidateProducts(IEnumerable<int> productIds)
    {
        var productIdList = productIds.ToList();
        var spec = new BaseSpecification<Product>
        {
            Criteria = p => productIdList.Contains(p.Id)
        };

        var products = (await _productRepository.GetAllAsync(spec)).ToList();

        if (products.Count != productIdList.Count)
            throw new Exception("One or more products do not exist.");

        return products;
    }

    private void ValidateSameMerchant(List<Product> products)
    {
        var merchantId = products.First().MerchantId;
        var hasMultipleMerchants = products.Any(p => p.MerchantId != merchantId);

        if (hasMultipleMerchants)
            throw new Exception("All products in an order must belong to the same merchant.");
    }

    private void ValidateProductAvailability(
        List<Product> products,
        Dictionary<int, int> orderProducts,
        int branchId)
    {
        var insufficientProducts = products
            .Where(p => GetBranchQuantity(p, branchId) < orderProducts[p.Id])
            .ToList();

        if (insufficientProducts.Any())
        {
            var errorMessage = BuildInsufficientStockMessage(
                insufficientProducts,
                orderProducts,
                branchId);
            throw new Exception(errorMessage);
        }
    }

    private int GetBranchQuantity(Product product, int branchId)
    {
        return product.ProductsBranches
            .FirstOrDefault(bp => bp.BranchId == branchId)?.Quantity ?? 0;
    }

    private string BuildInsufficientStockMessage(
        List<Product> insufficientProducts,
        Dictionary<int, int> orderProducts,
        int branchId)
    {
        var messages = insufficientProducts.Select(p =>
        {
            var available = GetBranchQuantity(p, branchId);
            var requested = orderProducts[p.Id];
            return $"Product '{p.Name}' has {available} available, but {requested} was requested.";
        });

        return string.Join(" | ", messages);
    }

    private async Task<Order> CreateOrder(CreateOrderDto dto, int customerId)
    {
        var order = _mapper.Map<Order>(dto);
        order.CustomerId = customerId;

        await _repository.CreateAsync(order);

        var orderCreated = await _unitOfWork.Complete();
        if (!orderCreated)
            throw new Exception("Failed to create order");

        return order;
    }
}
