using FreshBack.Domain.Interfaces.Repositories.Settings.PaymentMethods;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Settings.PaymentMethods;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Settings.PaymentMethods;

public class PaymentMethodRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<PaymentMethod> specificationCombiner) :
    BaseRepository<PaymentMethod, int>(context, specificationCombiner),
    IPaymentMethodRepository
{
}
