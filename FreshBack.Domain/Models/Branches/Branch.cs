using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.Products;
using FreshBack.Domain.Models.Settings.Areas;
using NetTopologySuite.Geometries;

namespace FreshBack.Domain.Models.Branches;

public class Branch : BaseImageModel<int>
{
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public string Neighborhood { get; set; } = default!;
    public string NeighborhoodEn { get; set; } = default!;
    public Point Location { get; set; } = default!;
    public TimeOnly OpeningTime { get; set; }
    public TimeOnly ClosingTime { get; set; }
    public BranchStatus Status { get; set; }
    public int AreaId { get; set; }
    public int MerchantId { get; set; }

    public Area Area { get; set; } = default!;
    public Merchant Merchant { get; set; } = default!;
    public IEnumerable<Product> Products { get; set; } = default!;
}
