using FreshBack.Application.Dtos.Carts;
using FreshBack.Application.Interfaces.Carts;
using FreshBack.Domain.Carts;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Carts;

[Route("api/[controller]")]
[ApiController]
public class CartItemsController(ICartItemService service) : BaseController<
    ICartItemService, CartItemDto, CartItemDto, CartItemDto, CartItemDto, CartItem,
    int>(service)
{
}
