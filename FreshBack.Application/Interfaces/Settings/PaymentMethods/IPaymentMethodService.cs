using FreshBack.Application.Dtos.Settings.PaymentMethods;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Settings.PaymentMethods;

namespace FreshBack.Application.Interfaces.Settings.PaymentMethods;

public interface IPaymentMethodService : IBaseService<
    PaymentMethodDto,
    PaymentMethodDto,
    PaymentMethodDto,
    PaymentMethodDto,
    PaymentMethod,
    int>
{
}
