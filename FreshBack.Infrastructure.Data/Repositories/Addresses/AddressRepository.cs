using FreshBack.Domain.Interfaces.Repositories.Addresses;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Addresses;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Addresses;

public class AddressRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<Address> specificationCombiner) :
    BaseRepository<Address, int>(context, specificationCombiner), IAddressRepository
{
}
