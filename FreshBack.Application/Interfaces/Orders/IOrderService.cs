using FreshBack.Application.Dtos.Orders;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Orders;

namespace FreshBack.Application.Interfaces.Orders;

public interface IOrderService : IBaseService<
    OrderDto,
    OrderDto,
    OrderDto,
    OrderDto,
    Order,
    int>
{
}
