using FreshBack.Application.Dtos.Products;
using FreshBack.Application.Interfaces.Products;
using FreshBack.Domain.Models.Products;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Products;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductService service) :
    BaseController<IProductService, ProductDto, ProductDto, ProductDto, ProductDto,
        Product, int>(service)
{
}
