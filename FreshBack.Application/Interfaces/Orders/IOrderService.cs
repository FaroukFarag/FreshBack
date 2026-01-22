using FreshBack.Application.Dtos.Orders;
using FreshBack.Application.Dtos.Shared;
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
    Task<ResultDto<CreateOrderDto>> CreateAsync(
        CreateOrderDto createOrderDto,
        int customerId);

    Task<ResultDto<PagedResult<OrderDto>>> GetCustomerOrders(
        GetCustomerPreviousOrdersDto getCustomerPreviousOrdersDto, int customerId);
}
