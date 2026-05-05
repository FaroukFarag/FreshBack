using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Domain.Enums.Orders;

namespace FreshBack.Application.Dtos.Orders;

public class UpdateOrderStatusDto : BaseModelDto<int>
{
    public OrderStatus Status { get; set; }
}
