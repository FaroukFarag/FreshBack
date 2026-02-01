using FreshBack.Domain.Enums.Merchants;
using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Orders;
using FreshBack.Domain.Models.Settings.Areas;

namespace FreshBack.Domain.Models.Merchants;

public class Merchant : BaseModel<int>
{
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string DescriptionEn { get; set; } = default!;
    public string Story { get; set; } = default!;
    public string StoryEn { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public MerchantStatus Status { get; set; }
    public int AreaId { get; set; }

    public Area Area { get; set; } = default!;
    public IEnumerable<Branch> Branches { get; set; } = default!;
    public IEnumerable<Order> Orders { get; set; } = default!;
    public IEnumerable<Review> Reviews { get; set; } = default!;
}
