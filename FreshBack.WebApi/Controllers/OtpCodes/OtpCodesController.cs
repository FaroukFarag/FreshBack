using FreshBack.Application.Dtos.OtpCodes;
using FreshBack.Application.Interfaces.OtpCodes;
using FreshBack.Domain.Models.OtpCodes;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.OtpCodes;

[Route("api/[controller]")]
[ApiController]
public class OtpCodesController(IOtpCodeService service) :
    BaseController<IOtpCodeService, OtpCodeDto, OtpCodeDto, OtpCodeDto, OtpCodeDto,
    OtpCode, int>(service)
{
    private readonly IOtpCodeService _service = service;

    [HttpPost("VerifyOtp")]
    public async Task<IActionResult> VerifyOtp(OtpCodeDto dto)
    {
        return Ok(await _service.VerifyOtp(dto));
    }

}
