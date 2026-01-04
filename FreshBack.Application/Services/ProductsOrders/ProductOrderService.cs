using AutoMapper;
using FreshBack.Application.Dtos.ProductsOrders;
using FreshBack.Application.Interfaces.ProductsOrders;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.ProductsOrders;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.ProductsOrders;

namespace FreshBack.Application.Services.ProductsOrders;

public class ProductOrderService(
    IProductOrderRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : BaseService<
        CreateProductOrderDto,
        ProductOrderDto,
        ProductOrderDto,
        ProductOrderDto,
        ProductOrder,
        (int ProductId, int OrderId)>(repository, unitOfWork, mapper),
    IProductOrderService
{
}
