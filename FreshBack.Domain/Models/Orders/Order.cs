using FreshBack.Domain.Enums.Orders;
using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Customers;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.ProductsOrders;

namespace FreshBack.Domain.Models.Orders;

public class Order : BaseModel<int>
{
    public int Number { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public OrderStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal Fees { get; set; }
    public int MerchantId { get; set; }
    public int BranchId { get; set; }
    public int CustomerId { get; set; }

    public Merchant Merchant { get; set; } = default!;
    public Branch Branch { get; set; } = default!;
    public Customer Customer { get; set; } = default!;
    public Review Review { get; set; } = default!;
    public IEnumerable<ProductOrder> ProductsOrders { get; set; } = default!;
}
