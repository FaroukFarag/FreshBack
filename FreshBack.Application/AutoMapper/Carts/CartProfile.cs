using AutoMapper;
using FreshBack.Application.Dtos.Carts;
using FreshBack.Domain.Models.Carts;

namespace FreshBack.Application.AutoMapper.Carts;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<Cart, CartDto>().ReverseMap();
    }
}
