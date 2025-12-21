using FreshBack.Domain.Interfaces.Repositories.Settings.Areas;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Settings.Areas;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Settings.Areas;

public class AreaRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<Area> specificationCombiner) :
    BaseRepository<Area, int>(context, specificationCombiner), IAreaRepository
{
}
