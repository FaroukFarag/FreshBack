using FreshBack.Application.Dtos.Orders;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Orders;

namespace FreshBack.Application.Interfaces.Orders;

public interface IOrderService : IBaseService<
    CreateOrderDto,
    OrderDto,
    OrderDto,
    OrderDto,
    Order,
    int>
{
}
