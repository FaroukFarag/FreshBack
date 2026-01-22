using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Customers;
using FreshBack.Domain.Models.Merchants;

namespace FreshBack.Domain.Models.Branches;

public class Review : BaseModel<int>
{
    public int Rating { get; set; }
    public string Comment { get; set; } = default!;
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public int MerchantId { get; set; }
    public int BranchId { get; set; }

    public Customer Customer { get; set; } = default!;
    public Merchant Merchant { get; set; } = default!;
    public Branch Branch { get; set; } = default!;
}
