using FreshBack.Domain.Enums.Orders;
using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Merchants;

namespace FreshBack.Domain.Models.Orders;

public class Order : BaseModel<int>
{
    public int Number { get; set; }
    public string CustomerName { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CustomerMobileNumber { get; set; } = default!;
    public DateTime CreationDate { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal Fees { get; set; }
    public int MerchantId { get; set; }

    public Merchant Merchant { get; set; } = default!;
}
