using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Customers;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Dtos.ProductsOrders;
using FreshBack.Domain.Enums.Orders;

namespace FreshBack.Application.Dtos.Orders;

public class OrderDto : BaseModelDto<int>
{
    public int Number { get; set; }
    public DateTime CreationDate { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal Fees { get; set; }
    public decimal? TotalAmount => ProductsOrders?
        .Sum(po => po.Quantity * po.Product?.Price) - Discount + Fees;
    public int MerchantId { get; set; }
    public int BranchId { get; set; }
    public int CustomerId { get; set; }

    public MerchantDto? Merchant { get; set; }
    public CustomerDto? Customer { get; set; }
    public IEnumerable<ProductOrderDto>? ProductsOrders { get; set; }
}
