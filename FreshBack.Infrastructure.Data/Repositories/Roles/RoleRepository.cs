using FreshBack.Domain.Interfaces.Repositories.Roles;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Roles;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Roles;

public class RoleRepository(
FreshBackDbContext context,
    ISpecificationCombiner<Role> specificationCombiner) :
    BaseRepository<Role, int>(context, specificationCombiner), IRoleRepository
{
}
