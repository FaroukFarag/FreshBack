using FreshBack.Domain.Interfaces.Repositories.OrdersPhotos;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.OrdersPhotos;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.OrdersPhotos;

public class OrderPhotoRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<OrderPhoto> specificationCombiner) : BaseRepository<OrderPhoto, int>(context, specificationCombiner),
    IOrderPhotoRepository
{
}
