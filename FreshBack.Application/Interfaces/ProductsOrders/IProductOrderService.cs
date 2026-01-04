using FreshBack.Application.Dtos.ProductsOrders;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.ProductsOrders;

namespace FreshBack.Application.Interfaces.ProductsOrders;

public interface IProductOrderService : IBaseService<
    ProductOrderDto,
    ProductOrderDto,
    ProductOrderDto,
    ProductOrderDto,
    ProductOrder,
    (int ProductId, int OrderId)>
{
}
