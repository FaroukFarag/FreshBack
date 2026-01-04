using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Models.Customers;

namespace FreshBack.Domain.Interfaces.Repositories.Customers;

public interface ICustomerRepository : IBaseRepository<Customer, int>
{
}
