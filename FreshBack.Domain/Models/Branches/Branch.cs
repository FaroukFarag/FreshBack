using FreshBack.Domain.Enums.Branches;
using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.BranchesFavorites;
using FreshBack.Domain.Models.BranchesProducts;
using FreshBack.Domain.Models.Categories;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.Orders;
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
    public int CategoryId { get; set; }

    public Area Area { get; set; } = default!;
    public Merchant Merchant { get; set; } = default!;
    public Category Category { get; set; } = default!;
    public IEnumerable<Order> Orders { get; set; } = default!;
    public IEnumerable<BranchProduct> BranchesProducts { get; set; } = default!;
    public IEnumerable<CustomerBranchFavorite> CustomersBranchesFavorite { get; set; } = default!;
    public IEnumerable<Review> Reviews { get; set; } = default!;
}
