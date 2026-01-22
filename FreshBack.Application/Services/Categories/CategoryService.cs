using AutoMapper;
using FreshBack.Application.Dtos.Categories;
using FreshBack.Application.Interfaces.Categories;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Categories;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Categories;

namespace FreshBack.Application.Services.Categories;

public class CategoryService(
    ICategoryRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<
        CreateCategoryDto,
        CategoryDto,
        CategoryDto,
        CategoryDto,
        Category,
        int>(repository, unitOfWork, mapper), ICategoryService
{
}
