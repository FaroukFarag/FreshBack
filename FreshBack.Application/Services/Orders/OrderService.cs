using AutoMapper;
using FreshBack.Application.Dtos.Notifications;
using FreshBack.Application.Dtos.Orders;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Firebase.Notifications;
using FreshBack.Application.Interfaces.Orders;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Common.Extensions;
using FreshBack.Domain.Enums.Notifications;
using FreshBack.Domain.Enums.Orders;
using FreshBack.Domain.Interfaces.Repositories.BranchesProducts;
using FreshBack.Domain.Interfaces.Repositories.Orders;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.BranchesProducts;
using FreshBack.Domain.Models.Orders;
using FreshBack.Domain.Models.ProductsOrders;
using FreshBack.Domain.Models.Shared;
using FreshBack.Domain.Specifications.Absraction;
using FreshBack.Domain.Specifications.Orders;

namespace FreshBack.Application.Services.Orders;

public class OrderService(
    IOrderRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IBranchProductRepository branchProductRepository,
    FirebaseNotificationSender firebaseNotificationSender)
    : BaseService<
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
    private readonly IBranchProductRepository _branchProductRepository = branchProductRepository;
    private readonly FirebaseNotificationSender _firebaseNotificationSender = firebaseNotificationSender;

    public async Task<ResultDto<CreateOrderDto>> CreateAsync(
        CreateOrderDto createOrderDto,
        int customerId)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Order",
            action: async () =>
            {
                var orderProducts = BuildOrderProductsDictionary(createOrderDto);
                var branchesProducts = await GetAndValidateProducts(
                    orderProducts.Keys, createOrderDto.BranchId);

                ValidateSameBranch(branchesProducts);
                ValidateProductExpiration(branchesProducts);
                ValidateProductAvailability(branchesProducts, orderProducts);

                var order = await CreateOrder(
                    createOrderDto, customerId, branchesProducts, orderProducts);

                UpdateProductQuantities(branchesProducts, orderProducts);

                _branchProductRepository.UpdateRange(branchesProducts);

                var orderCreated = await _unitOfWork.Complete();

                if (!orderCreated)
                    throw new Exception("Failed to create order");

                return createOrderDto;
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
                    Includes =
                    [
                        o => o.Branch,
                        o => o.Merchant
                    ],
                    IncludeChains =
                    [
                        new IncludeChain<Order>
                        {
                            InitialInclude = o => o.ProductsOrders,
                            ThenIncludes =
                            [
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
                        o => o.Branch,
                        o => o.Merchant
                    ]
                };

                var (orders, totalCount) = await _repository.GetAllPaginatedAsync(
                    _mapper.Map<PaginatedModel>(paginatedModelDto),
                    spec);

                return new PagedResult<OrderDto>(
                    _mapper.Map<IEnumerable<OrderDto>>(orders),
                    totalCount);
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

                var paginatedModel = _mapper.Map<PaginatedModel>(getCustomerPreviousOrdersDto);

                var (orders, totalCount) = await _repository.GetAllPaginatedAsync(
                    paginatedModel,
                    spec);

                return new PagedResult<OrderDto>(
                    _mapper.Map<IEnumerable<OrderDto>>(orders),
                    totalCount);
            });
    }

    public async Task<ResultDto<OrderDto>> UpdateOrderStatus(
        UpdateOrderStatusDto updateOrderStatusDto)
    {
        return await ExecuteServiceCallAsync(
            "Get All Branches Paginated",
            async () =>
            {
                var order = await _repository.GetAsync(
                    updateOrderStatusDto.Id,
                    new BaseSpecification<Order>
                    {
                        Includes =
                        [
                            o => o.Merchant,
                            o => o.Branch
                        ]
                    });

                order.Status = updateOrderStatusDto.Status;

                order = _repository.Update(order);

                var orderUpdated = await _unitOfWork.Complete();

                if (!orderUpdated)
                    throw new Exception("Failed to update order status");

                if (order.Status == OrderStatus.Confirmed)
                    await _firebaseNotificationSender.SendToCustomerAsync(
                        order.CustomerId,
                        new OrderConfirmedNotificationDto
                        {
                            Receiver = NotificationReceiver.Customer,
                            Title = "Review your Order",
                            Content = "Your order has been confirmed. Please take a moment to " +
                                "review your experience.",
                            Order = new OrderConfirmedDto
                            {
                                Id = order.Id,
                                BranchName = order.Branch.Name,
                                MerchantName = order.Merchant.Name
                            }
                        });

                return _mapper.Map<OrderDto>(order);
            });
    }

    private static Dictionary<int, int> BuildOrderProductsDictionary(CreateOrderDto dto)
    {
        return dto.ProductsOrders!.ToDictionary(x => x.ProductId, x => x.Quantity);
    }

    private async Task<IEnumerable<BranchProduct>> GetAndValidateProducts(
        IEnumerable<int> productIds, int branchId)
    {
        var spec = new BaseSpecification<BranchProduct>
        {
            Criteria = bp => productIds.Contains(bp.ProductId) && bp.BranchId == branchId,
            Includes =
            [
                p => p.Product
            ]
        };

        var branchesProducts = (await _branchProductRepository.GetAllAsync(spec));

        if (branchesProducts.Count() != productIds.Count())
            throw new Exception("One or more products do not exist.");

        return branchesProducts;
    }

    private static void ValidateSameBranch(IEnumerable<BranchProduct> branchesProducts)
    {
        var branchId = branchesProducts.First().BranchId;
        var hasMultipleMerchants = branchesProducts.Any(p => p.BranchId != branchId);

        if (hasMultipleMerchants)
            throw new Exception("All products in an order must belong to the same branch.");
    }

    private void ValidateProductExpiration(IEnumerable<BranchProduct> branchesProducts)
    {
        var expiredProducts = branchesProducts
            .Where(p => p.ExpiryDate < DateTime.Now);

        if (expiredProducts.Any())
        {
            var errorMessage = BuildExpiredProductsMessage(expiredProducts);

            throw new Exception(errorMessage);
        }
    }

    private void ValidateProductAvailability(
        IEnumerable<BranchProduct> products,
        Dictionary<int, int> orderProducts)
    {
        var insufficientProducts = products
            .Where(p => p.Quantity < orderProducts[p.ProductId]);

        if (insufficientProducts.Any())
        {
            var errorMessage = BuildInsufficientStockMessage(
                insufficientProducts,
                orderProducts);

            throw new Exception(errorMessage);
        }
    }

    private static void UpdateProductQuantities(
        IEnumerable<BranchProduct> branchesProducts,
        Dictionary<int, int> orderProducts)
    {
        foreach (var branchProduct in branchesProducts)
        {
            var orderedQuantity = orderProducts[branchProduct.ProductId];

            branchProduct.Quantity -= orderedQuantity;
        }
    }

    private static string BuildExpiredProductsMessage(IEnumerable<BranchProduct> expiredProducts)
    {
        var messages = expiredProducts.Select(p =>
            $"Product '{p.Product.Name}' expired on {p.ExpiryDate:yyyy-MM-dd}.");

        return string.Join(" | ", messages);
    }

    private static string BuildInsufficientStockMessage(
        IEnumerable<BranchProduct> insufficientProducts,
        Dictionary<int, int> orderProducts)
    {
        var messages = insufficientProducts.Select(p =>
        {
            var available = p.Quantity;
            var requested = orderProducts[p.ProductId];
            return $"Product '{p.Product.Name}' has {available} available, but {requested} was requested.";
        });

        return string.Join(" | ", messages);
    }

    private async Task<Order> CreateOrder(
        CreateOrderDto dto,
        int customerId,
        IEnumerable<BranchProduct> branchProducts,
        Dictionary<int, int> orderProducts)
    {
        var order = _mapper.Map<Order>(dto);

        order.CustomerId = customerId;

        order.ProductsOrders = branchProducts
        .Select(bp => new ProductOrder
        {
            ProductId = bp.ProductId,
            Quantity = orderProducts[bp.ProductId],

            Price = bp.Product.Price
        })
        .ToList();

        return await _repository.CreateAsync(order);
    }
}