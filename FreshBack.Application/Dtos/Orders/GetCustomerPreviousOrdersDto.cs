using FreshBack.Application.Dtos.Shared;
using FreshBack.Domain.Enums.Orders;
using FreshBack.Domain.Enums.Shared;

namespace FreshBack.Application.Dtos.Orders;

public class GetCustomerPreviousOrdersDto : PaginatedModelDto
{
    public OrderStatus Status { get; set; }
    public OrderSortBy SortBy { get; set; } = OrderSortBy.Date;
    public SortDirection SortDirection { get; set; } = SortDirection.Asc;
}
