using FreshBack.Application.Dtos.Categories;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Categories;
using FreshBack.Domain.Models.Categories;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Categories;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryService service) :
    BaseController<ICategoryService, CreateCategoryDto, CategoryDto, CategoryDto,
        CategoryDto, Category, int>(service)
{
    [AllowAnonymous]
    public override Task<IActionResult> GetAllPaginated(PaginatedModelDto paginatedModelDto)
    {
        return base.GetAllPaginated(paginatedModelDto);
    }
}
