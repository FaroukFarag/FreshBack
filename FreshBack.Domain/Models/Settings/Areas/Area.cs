using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Branches;

namespace FreshBack.Domain.Models.Settings.Areas;

public class Area : BaseModel<int>
{
    public string Name { get; set; } = default!;
    public decimal DeliveryFees { get; set; }

    public IEnumerable<Branch> Branches { get; set; } = default!;
}
