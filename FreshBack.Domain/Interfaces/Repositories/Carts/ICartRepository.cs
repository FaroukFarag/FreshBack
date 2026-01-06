using FreshBack.Domain.Carts;
using FreshBack.Domain.Interfaces.Repositories.Abstraction;

namespace FreshBack.Domain.Interfaces.Repositories.Carts;

public interface ICartRepository : IBaseRepository<Cart, int>
{
}
