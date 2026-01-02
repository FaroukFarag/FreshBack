using FreshBack.Application.Dtos.Products;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Products;

namespace FreshBack.Application.Interfaces.Products;

public interface IProductService : IBaseService<
    CreateProductDto,
    ProductDto,
    ProductDto,
    ProductDto,
    Product,
    int>
{
}
