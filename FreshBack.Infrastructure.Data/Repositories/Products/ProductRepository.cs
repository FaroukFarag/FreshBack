using FreshBack.Domain.Interfaces.Repositories.Products;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Products;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Products;

public class ProductRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<Product> specificationCombiner) :
    BaseRepository<Product, int>(context, specificationCombiner), IProductRepository
{
}
