using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Customers;

namespace FreshBack.Domain.Models.BranchesFavorites;

public class CustomerBranchFavorite
{
    public int BranchId { get; set; }
    public int CustomerId { get; set; }

    public Branch Branch { get; set; } = default!;
    public Customer Customer { get; set; } = default!;
}
