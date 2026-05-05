using FreshBack.Domain.Models.Abstraction;

namespace FreshBack.Domain.Models.Settings.Areas;

public class Area : BaseAuditModel<int>
{
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public decimal DeliveryFees { get; set; }
    public bool IsActive { get; set; }
}
