using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Models.Carts;

namespace FreshBack.Domain.Interfaces.Repositories.Carts;

public interface ICartRepository : IBaseRepository<Cart, int>
{
}
