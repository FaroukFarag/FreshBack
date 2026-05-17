using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Domain.Enums.Orders;

namespace FreshBack.Application.Dtos.Orders;

public class GetOrderDto : BaseModelDto<int>
{
    public int Number { get; set; }
    public DateTime CreationDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal Fees { get; set; }
    public decimal OrderFinalAmount { get; set; }
    public int PaymentMethodId { get; set; }
    public int MerchantId { get; set; }
    public int BranchId { get; set; }
    public int CustomerId { get; set; }
}
