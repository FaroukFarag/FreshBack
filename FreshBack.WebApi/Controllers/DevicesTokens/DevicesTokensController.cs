using FreshBack.Application.Dtos.DevicesTokens;
using FreshBack.Application.Interfaces.DevicesTokens;
using FreshBack.Domain.Models.DevicesTokens;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.DevicesTokens;

[Route("api/[controller]")]
[ApiController]
public class DevicesTokensController(IDeviceTokenService service) :
    BaseController<IDeviceTokenService, CreateDeviceTokenDto, DeviceTokenDto, DeviceTokenDto,
        DeviceTokenDto, DeviceToken, int>(service)
{
}
