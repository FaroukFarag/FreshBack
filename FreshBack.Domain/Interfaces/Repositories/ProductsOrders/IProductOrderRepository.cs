using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Models.ProductsOrders;

namespace FreshBack.Domain.Interfaces.Repositories.ProductsOrders;

public interface IProductOrderRepository : IBaseRepository<ProductOrder,
    (int ProductId, int OrderId)>
{
}
