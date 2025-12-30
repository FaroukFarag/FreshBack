using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Models.Orders;

namespace FreshBack.Domain.Interfaces.Repositories.Orders;

public interface IOrderRepository : IBaseRepository<Order, int>
{
}
