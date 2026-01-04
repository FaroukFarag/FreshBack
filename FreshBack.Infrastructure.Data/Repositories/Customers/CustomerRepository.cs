using FreshBack.Domain.Interfaces.Repositories.Customers;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Customers;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Customers;

public class CustomerRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<Customer> specificationCombiner) :
    BaseRepository<Customer, int>(context, specificationCombiner), ICustomerRepository
{
}
