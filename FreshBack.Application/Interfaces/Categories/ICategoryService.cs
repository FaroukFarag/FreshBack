using FreshBack.Application.Dtos.Categories;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Categories;

namespace FreshBack.Application.Interfaces.Categories;

public interface ICategoryService : IBaseService<
    CreateCategoryDto,
    CategoryDto,
    CategoryDto,
    CategoryDto,
    Category,
    int>
{
}
