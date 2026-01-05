using FreshBack.Application.Dtos.Categories;
using FreshBack.Application.Interfaces.Categories;
using FreshBack.Domain.Models.Categories;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Categories;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryService service) :
    BaseController<ICategoryService, CategoryDto, CategoryDto, CategoryDto,
        CategoryDto, Category, int>(service)
{
}
