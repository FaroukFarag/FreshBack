using FreshBack.Domain.Models.Orders;
using FreshBack.Domain.Models.Products;
using FreshBack.Domain.Shared.Attributs;

namespace FreshBack.Domain.Models.ProductsOrders;

public class ProductOrder
{
    [CompositeKey]
    public int ProductId { get; set; }

    [CompositeKey]
    public int OrderId { get; set; }

    public int Quantity { get; set; }

    public Product Product { get; set; } = default!;
    public Order Order { get; set; } = default!;
}
