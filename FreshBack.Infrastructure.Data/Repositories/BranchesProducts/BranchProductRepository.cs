using FreshBack.Domain.Interfaces.Repositories.BranchesProducts;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.BranchesProducts;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.BranchesProducts;

public class BranchProductRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<BranchProduct> specificationCombiner) :
    BaseRepository<
        BranchProduct, (int BranchId, int ProductId)>(context, specificationCombiner),
    IBranchProductRepository
{
}
