using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Dtos.Customers;

namespace FreshBack.Application.Dtos.CustomersBranchesFavorite;

public class CustomerBranchFavoriteDto
{
    public int BranchId { get; set; }
    public int CustomerId { get; set; }

    public CustomerDto Customer { get; set; } = default!;
    public BranchDto Branch { get; set; } = default!;
}
