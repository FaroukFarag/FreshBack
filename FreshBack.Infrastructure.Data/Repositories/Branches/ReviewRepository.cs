using FreshBack.Domain.Interfaces.Repositories.Branches;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Branches;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Branches;

public class ReviewRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<Review> specificationCombiner) :
    BaseRepository<Review, int>(context, specificationCombiner), IReviewRepository
{
}
