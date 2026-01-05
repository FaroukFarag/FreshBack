using AutoMapper;
using FreshBack.Application.Dtos.Categories;
using FreshBack.Domain.Models.Categories;

namespace FreshBack.Application.AutoMapper.Categories;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}
