using FreshBack.Domain.Interfaces.Repositories.Merchants;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Merchants;

public class ReviewRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<Review> specificationCombiner) :
    BaseRepository<Review, int>(context, specificationCombiner), IReviewRepository
{
}
