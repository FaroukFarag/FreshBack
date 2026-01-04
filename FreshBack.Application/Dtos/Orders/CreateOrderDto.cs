using FreshBack.Application.Dtos.ProductsOrders;
using FreshBack.Domain.Enums.Orders;

namespace FreshBack.Application.Dtos.Orders;

public class CreateOrderDto
{
    public int Number { get; set; }
    public DateTime CreationDate { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal Fees { get; set; }
    public int MerchantId { get; set; }
    public int CustomerId { get; set; }

    public IEnumerable<CreateProductOrderDto>? ProductsOrders { get; set; }
}
