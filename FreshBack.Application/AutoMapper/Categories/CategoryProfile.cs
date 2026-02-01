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
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageDtoUrlResolver>());

        CreateMap<Category, CreateCategoryDto>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageDtoUrlResolver>());

        CreateMap<CategoryDto, Category>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageUrlResolver>());

        CreateMap<CreateCategoryDto, Category>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageUrlResolver>());
    }
}
