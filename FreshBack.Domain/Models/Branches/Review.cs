using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Customers;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.Orders;

namespace FreshBack.Domain.Models.Branches;

public class Review : BaseImageModel<int>
{
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public int CustomerId { get; set; }
    public int MerchantId { get; set; }
    public int BranchId { get; set; }
    public int OrderId { get; set; } = default!;

    public Customer Customer { get; set; } = default!;
    public Merchant Merchant { get; set; } = default!;
    public Branch Branch { get; set; } = default!;
    public Order Order { get; set; } = default!;
}
