using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Domain.Enums.Merchants;
using FreshBack.Domain.Models.Settings.Areas;

namespace FreshBack.Application.Dtos.Merchants;

public class MerchantDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Story { get; set; } = default!;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Location { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public TimeOnly OpeningTime { get; set; }
    public TimeOnly ClosingTime { get; set; }
    public MerchantStatus Status { get; set; }
    public int AreaId { get; set; }

    public Area? Area { get; set; }
    public IEnumerable<ReviewDto>? Reviews { get; set; }
}
