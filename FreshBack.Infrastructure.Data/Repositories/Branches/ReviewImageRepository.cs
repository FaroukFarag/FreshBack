using FreshBack.Domain.Interfaces.Repositories.Branches;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Branches;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Branches;

public class ReviewImageRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<ReviewImage> specificationCombiner) :
    BaseRepository<ReviewImage, int>(context, specificationCombiner), IReviewImageRepository
{
}
