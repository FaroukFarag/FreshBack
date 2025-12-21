using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Models.Settings.Users;

namespace FreshBack.Domain.Interfaces.Repositories.Settings.Users;

public interface IUserRepository : IBaseRepository<User, int>
{
}
