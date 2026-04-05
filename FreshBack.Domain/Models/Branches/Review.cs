using FreshBack.Common.Interfaces.Merchants;
using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Customers;
using FreshBack.Domain.Models.Merchants;

namespace FreshBack.Domain.Models.Branches;

public class Review : BaseAuditModel<int>, IMerchantEntity
{
    public int? Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public int CustomerId { get; set; }
    public int MerchantId { get; set; }
    public int BranchId { get; set; }

    public Customer Customer { get; set; } = default!;
    public Merchant Merchant { get; set; } = default!;
    public Branch Branch { get; set; } = default!;
    public IEnumerable<ReviewImage> ReviewImages { get; set; } = default!;
}
