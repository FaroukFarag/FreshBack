using FreshBack.Domain.Enums.Merchants;
using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Categories;
using FreshBack.Domain.Models.Orders;

namespace FreshBack.Domain.Models.Merchants;

public class Merchant : BaseImageAuditModel<int>
{
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string DescriptionEn { get; set; } = default!;
    public string Story { get; set; } = default!;
    public string StoryEn { get; set; } = default!;
    public int CategoryId { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public MerchantStatus Status { get; set; }

    public Category Category { get; set; } = default!;
    public IEnumerable<Branch> Branches { get; set; } = default!;
    public IEnumerable<Order> Orders { get; set; } = default!;
    public IEnumerable<Review> Reviews { get; set; } = default!;
}
