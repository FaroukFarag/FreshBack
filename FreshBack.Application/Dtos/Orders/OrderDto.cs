using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Domain.Enums.Orders;

namespace FreshBack.Application.Dtos.Orders;

public class OrderDto : BaseModelDto<int>
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
    public decimal TotalAmount => Price - Discount + Fees;
    public int MerchantId { get; set; }

    public MerchantDto Merchant { get; set; } = default!;
}
