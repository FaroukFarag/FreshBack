using FreshBack.Application.Dtos.Carts;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Carts;

namespace FreshBack.Application.Interfaces.Carts;

public interface ICartService : IBaseService<
    CartDto,
    CartDto,
    CartDto,
    CartDto,
    Cart,
    int>
{
}
