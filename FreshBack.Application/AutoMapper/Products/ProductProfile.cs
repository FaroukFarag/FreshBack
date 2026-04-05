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
         .ForMember(dest => dest.ImagePath, opt => opt
             .MapFrom<ImagePathToUrlResolver<ProductImage, ProductImageDto>>());

        CreateMap<ProductImage, CreateProductImageDto>()
            .ForMember(dest => dest.ImagePath, opt => opt
                .MapFrom<ImagePathToUrlResolver<ProductImage, CreateProductImageDto>>());

        CreateMap<ProductImageDto, ProductImage>()
            .ForMember(dest => dest.ImagePath, opt => opt
                .MapFrom<ImageUrlToPathResolver<ProductImageDto, ProductImage>>());

        CreateMap<CreateProductImageDto, ProductImage>()
            .ForMember(dest => dest.ImagePath, opt => opt
                .MapFrom<ImageUrlToPathResolver<CreateProductImageDto, ProductImage>>());

        CreateMap<Product, ProductDto>().ReverseMap();

        CreateMap<CreateProductDto, Product>().ReverseMap();
    }
}
