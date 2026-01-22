using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Models.BranchesFavorites;

namespace FreshBack.Domain.Interfaces.Repositories.CustomersBranchesFavorite;

public interface ICustomerBranchFavoriteRepository :
    IBaseRepository<CustomerBranchFavorite,
    (int CustomerId, int BranchId)>
{
}
