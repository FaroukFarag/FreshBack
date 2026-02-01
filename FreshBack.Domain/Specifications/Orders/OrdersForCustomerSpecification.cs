using FreshBack.Domain.Enums.Orders;
using FreshBack.Domain.Enums.Shared;
using FreshBack.Domain.Models.Orders;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Domain.Specifications.Orders;

public sealed class OrdersForCustomerSpecification : BaseSpecification<Order>
{
    public OrdersForCustomerSpecification(
        int customerId,
        OrderStatus status,
        OrderSortBy sortBy,
        SortDirection sortDirection)
    {
        ApplyCriteria(customerId, status);
        ApplyIncludes();
        ApplySorting(sortBy, sortDirection);
    }

    private void ApplyCriteria(int customerId, OrderStatus status)
    {
        Criteria = b => b.CustomerId == customerId && b.Status == status;
    }

    private void ApplyIncludes()
    {
        AddInclude(b => b.Merchant);
        AddInclude(b => b.Branch);
    }

    private void ApplySorting(OrderSortBy sortBy, SortDirection direction)
    {
        switch (sortBy)
        {
            case OrderSortBy.Price:
                ApplyOrder(o => o.ProductsOrders.Min(po => po.Product.Price), direction);
                break;

            case OrderSortBy.Category:
                ApplyOrder(o => o.Branch.Category.Name, direction);
                break;

            case OrderSortBy.Date:
            default:
                ApplyOrder(o => o.CreationDate, direction);
                break;
        }
    }
}
