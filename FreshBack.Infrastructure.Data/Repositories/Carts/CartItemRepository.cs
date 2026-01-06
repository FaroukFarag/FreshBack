using FreshBack.Domain.Carts;
using FreshBack.Domain.Interfaces.Repositories.Carts;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Carts;

public class CartItemRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<CartItem> specificationCombiner) :
    BaseRepository<CartItem, int>(context, specificationCombiner), ICartItemRepository
{
}
