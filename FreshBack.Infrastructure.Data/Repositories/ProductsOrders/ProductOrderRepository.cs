using FreshBack.Domain.Interfaces.Repositories.ProductsOrders;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.ProductsOrders;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.ProductsOrders;

public class ProductOrderRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<ProductOrder> specificationCombiner) :
    BaseRepository<ProductOrder, (int ProductId, int OrderId)>(
        context, specificationCombiner),
    IProductOrderRepository
{
}
