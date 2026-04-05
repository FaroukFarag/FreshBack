using AutoMapper;
using FreshBack.Application.Dtos.Settings.PaymentMethods;
using FreshBack.Application.Interfaces.Settings.PaymentMethods;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Settings.PaymentMethods;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Settings.PaymentMethods;

namespace FreshBack.Application.Services.Settings.PaymentMethods;

public class PaymentMethodService(
    IPaymentMethodRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : BaseService<PaymentMethodDto, PaymentMethodDto, PaymentMethodDto,
        PaymentMethodDto, PaymentMethod, int>(repository, unitOfWork, mapper),
    IPaymentMethodService
{
}
