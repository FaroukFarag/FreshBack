using FreshBack.Application.Dtos.Orders;
using FreshBack.Application.Dtos.Products;

namespace FreshBack.Application.Dtos.ProductsOrders;

public class ProductOrderDto
{
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }

    public ProductDto Product { get; set; } = default!;
    public OrderDto Order { get; set; } = default!;
}
