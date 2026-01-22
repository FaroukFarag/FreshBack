using FreshBack.Domain.Interfaces.Repositories.CustomersBranchesFavorite;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.BranchesFavorites;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.CustomersBranchesFavorite;

public class CustomerBranchFavoriteRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<CustomerBranchFavorite> specificationCombiner) :
    BaseRepository<CustomerBranchFavorite,
        (int CustomerId, int BranchId)>(context, specificationCombiner),
    ICustomerBranchFavoriteRepository
{
}
