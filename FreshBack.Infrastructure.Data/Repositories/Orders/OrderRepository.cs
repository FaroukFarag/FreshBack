using FreshBack.Domain.Interfaces.Repositories.Orders;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Orders;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Orders;

public class OrderRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<Order> specificationCombiner) :
    BaseRepository<Order, int>(context, specificationCombiner), IOrderRepository
{
}
