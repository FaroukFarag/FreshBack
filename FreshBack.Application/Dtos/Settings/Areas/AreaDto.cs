using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Settings.Areas;

public class AreaDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
    public decimal DeliveryFees { get; set; }
}
