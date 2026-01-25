using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Customers;
using FreshBack.Domain.Shared.Attributs;

namespace FreshBack.Domain.Models.BranchesFavorites;

public class CustomerBranchFavorite
{
    [CompositeKey]
    public int CustomerId { get; set; }

    [CompositeKey]
    public int BranchId { get; set; }

    public Branch Branch { get; set; } = default!;
    public Customer Customer { get; set; } = default!;
}
