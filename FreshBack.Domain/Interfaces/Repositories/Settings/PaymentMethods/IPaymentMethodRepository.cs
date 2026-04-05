using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Models.Settings.PaymentMethods;

namespace FreshBack.Domain.Interfaces.Repositories.Settings.PaymentMethods;

public interface IPaymentMethodRepository : IBaseRepository<PaymentMethod, int>
{
}
