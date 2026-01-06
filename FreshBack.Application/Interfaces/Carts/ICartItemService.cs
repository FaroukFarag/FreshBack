using FreshBack.Application.Dtos.Carts;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Carts;

namespace FreshBack.Application.Interfaces.Carts;

public interface ICartItemService : IBaseService<
    CartItemDto,
    CartItemDto,
    CartItemDto,
    CartItemDto,
    CartItem,
    int>
{
}
