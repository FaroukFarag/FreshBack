using FreshBack.Application.Dtos.ProductsOrders;
using FreshBack.Domain.Enums.Orders;

namespace FreshBack.Application.Dtos.Orders;

public class CreateOrderDto
{
    public int Number { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Discount { get; set; }
    public int PaymentMethodId { get; set; }
    public int MerchantId { get; set; }
    public int BranchId { get; set; }

    public IEnumerable<CreateProductOrderDto>? ProductsOrders { get; set; }
}
