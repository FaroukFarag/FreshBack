using FreshBack.Domain.Interfaces.Repositories.Branches;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Branches;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Branches;

public class BranchRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<Branch> specificationCombiner) :
    BaseRepository<Branch, int>(context, specificationCombiner), IBranchRepository
{
}
