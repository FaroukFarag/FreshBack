using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Interfaces.Merchants;
using FreshBack.Domain.Models.Merchants;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Merchants;

[Route("api/[controller]")]
[ApiController]
public class MerchantsController(IMerchantService service) :
    BaseController<IMerchantService, MerchantDto, MerchantDto, MerchantDto,
        MerchantDto, Merchant, int>(service)
{
}
