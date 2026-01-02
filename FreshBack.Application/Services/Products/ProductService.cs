using AutoMapper;
using FreshBack.Application.Dtos.Products;
using FreshBack.Application.Interfaces.Products;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Products;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Products;

namespace FreshBack.Application.Services.Products;

public class ProductService(
    IProductRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : BaseService<
    CreateProductDto,
    ProductDto,
    ProductDto,
    ProductDto,
    Product,
    int>(repository, unitOfWork, mapper), IProductService
{
}
