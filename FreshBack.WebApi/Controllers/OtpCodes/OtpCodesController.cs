using FreshBack.Application.Dtos.OtpCodes;
using FreshBack.Application.Interfaces.OtpCodes;
using FreshBack.Domain.Models.OtpCodes;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.OtpCodes;

[Route("api/[controller]")]
[ApiController]
public class OtpCodesController(IOtpCodeService service) :
    BaseController<IOtpCodeService, OtpCodeDto, OtpCodeDto, OtpCodeDto, OtpCodeDto,
    OtpCode, int>(service)
{
    private readonly IOtpCodeService _service = service;

    [AllowAnonymous]
    public override Task<IActionResult> Create(OtpCodeDto createEntityDto)
    {
        return base.Create(createEntityDto);
    }

    [HttpPost("VerifyOtp")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyOtp(OtpCodeDto dto)
    {
        return Ok(await _service.VerifyOtp(dto));
    }

}
