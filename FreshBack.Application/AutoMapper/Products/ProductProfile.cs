using AutoMapper;
using FreshBack.Application.AutoMapper.Resolvers;
using FreshBack.Application.Dtos.Products;
using FreshBack.Domain.Models.Products;

namespace FreshBack.Application.AutoMapper.Products;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductImage, ProductImageDto>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageDtoUrlResolver>());

        CreateMap<ProductImageDto, ProductImage>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageUrlResolver>());

        CreateMap<ProductImage, CreateProductImageDto>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageDtoUrlResolver>());

        CreateMap<CreateProductImageDto, ProductImage>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageUrlResolver>());

        CreateMap<Product, ProductDto>().ReverseMap();

        CreateMap<CreateProductDto, Product>().ReverseMap();
    }
}
