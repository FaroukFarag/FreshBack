using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Models.Carts;

namespace FreshBack.Domain.Interfaces.Repositories.Carts;

public interface ICartItemRepository : IBaseRepository<CartItem, int>
{
}
