using FreshBack.Domain.Interfaces.Repositories.Users;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Users;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Users;
public class UserRepository(
FreshBackDbContext context,
    ISpecificationCombiner<User> specificationCombiner) :
    BaseRepository<User, int>(context, specificationCombiner), IUserRepository
{
}
