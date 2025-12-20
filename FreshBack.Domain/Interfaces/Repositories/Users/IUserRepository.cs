using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Models.Users;

namespace FreshBack.Domain.Interfaces.Repositories.Users;

public interface IUserRepository : IBaseRepository<User, int>
{
}
