using AutoMapper;
using FreshBack.Application.Dtos.Carts;
using FreshBack.Application.Interfaces.Carts;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Carts;
using FreshBack.Domain.Interfaces.Repositories.Carts;
using FreshBack.Domain.Interfaces.UnitOfWork;

namespace FreshBack.Application.Services.Carts;

public class CartItemService(
    ICartItemRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<
        CartItemDto,
        CartItemDto,
        CartItemDto,
        CartItemDto,
        CartItem,
        int>(repository, unitOfWork, mapper), ICartItemService
{
}
