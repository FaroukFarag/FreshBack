using FreshBack.Domain.Interfaces.Repositories.Settings.Commissions;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Settings.Commissions;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Settings.Commissions;

public class CategoryCommissionRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<CategoryCommission> specificationCombiner) :
    BaseRepository<CategoryCommission, int>(context, specificationCombiner),
    ICategoryCommissionRepository
{
}
