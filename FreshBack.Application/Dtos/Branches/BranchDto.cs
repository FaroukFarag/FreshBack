using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Dtos.Settings.Areas;
using FreshBack.Domain.Models.Branches;

namespace FreshBack.Application.Dtos.Branches;

public class BranchDto : BaseImageModelDto<int>
{
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public string Neighborhood { get; set; } = default!;
    public string NeighborhoodEn { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public TimeOnly OpeningTime { get; set; }
    public TimeOnly ClosingTime { get; set; }
    public BranchStatus Status { get; set; }
    public int AreaId { get; set; }
    public int MerchantId { get; set; }

    public AreaDto Area { get; set; } = default!;
    public MerchantDto Merchant { get; set; } = default!;
}
