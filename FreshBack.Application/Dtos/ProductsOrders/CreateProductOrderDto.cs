namespace FreshBack.Application.Dtos.ProductsOrders;

public class CreateProductOrderDto
{
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }
}
