using AutoMapper;
using FreshBack.Application.AutoMapper.Resolvers;
using FreshBack.Application.Dtos.Categories;
using FreshBack.Domain.Models.Categories;

namespace FreshBack.Application.AutoMapper.Categories;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.ImagePath, opt => opt
                .MapFrom<ImagePathToUrlResolver<Category, CategoryDto>>());

        CreateMap<Category, CreateCategoryDto>()
            .ForMember(dest => dest.ImagePath, opt => opt
                .MapFrom<ImagePathToUrlResolver<Category, CreateCategoryDto>>());

        CreateMap<CategoryDto, Category>()
            .ForMember(dest => dest.ImagePath, opt => opt
                .MapFrom<ImageUrlToPathResolver<CategoryDto, Category>>());

        CreateMap<CreateCategoryDto, Category>()
            .ForMember(dest => dest.ImagePath, opt => opt
                .MapFrom<ImageUrlToPathResolver<CreateCategoryDto, Category>>());
    }
}
