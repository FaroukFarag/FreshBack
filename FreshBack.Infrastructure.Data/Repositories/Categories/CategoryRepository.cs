using FreshBack.Domain.Interfaces.Repositories.Categories;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Categories;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Categories;

public class CategoryRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<Category> specificationCombiner) :
    BaseRepository<Category, int>(context, specificationCombiner), ICategoryRepository
{
}
