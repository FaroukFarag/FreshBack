using AutoMapper;
using FreshBack.Application.Dtos.Carts;
using FreshBack.Domain.Models.Carts;

namespace FreshBack.Application.AutoMapper.Carts;

public class CartItemProfile : Profile
{
    public CartItemProfile()
    {
        CreateMap<CartItem, CartItemDto>().ReverseMap(); ;
    }
}
