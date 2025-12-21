using FreshBack.Domain.Models.Abstraction;

namespace FreshBack.Domain.Models.Settings.Areas;

public class Area : BaseModel<int>
{
    public string Name { get; set; } = default!;
    public decimal DeliveryFees { get; set; }
}
