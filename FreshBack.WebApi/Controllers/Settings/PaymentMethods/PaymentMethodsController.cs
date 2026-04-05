using FreshBack.Application.Dtos.Settings.PaymentMethods;
using FreshBack.Application.Interfaces.Settings.PaymentMethods;
using FreshBack.Domain.Models.Settings.PaymentMethods;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Settings.PaymentMethods;

[Route("api/[controller]")]
[ApiController]
public class PaymentMethodsController(IPaymentMethodService service) :
    BaseController<IPaymentMethodService, PaymentMethodDto, PaymentMethodDto, PaymentMethodDto,
        PaymentMethodDto, PaymentMethod, int>(service)
{
}
