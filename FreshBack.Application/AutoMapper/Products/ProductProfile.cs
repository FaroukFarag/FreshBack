using AutoMapper;
using FreshBack.Application.Dtos.Products;
using FreshBack.Domain.Models.Products;

namespace FreshBack.Application.AutoMapper.Products;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}
