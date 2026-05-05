using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Interfaces.Merchants;
using FreshBack.Domain.Models.Merchants;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Merchants;

[Route("api/[controller]")]
[ApiController]
public class MerchantsController(IMerchantService service) :
    BaseController<IMerchantService, CreateMerchantDto, MerchantDto, MerchantDto,
        MerchantDto, Merchant, int>(service)
{
    private readonly IMerchantService _service = service;

    public override Task<IActionResult> Create([FromForm] CreateMerchantDto createEntityDto)
    {
        return base.Create(createEntityDto);
    }

    [HttpPatch("UpdateStatus")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusDto updateStatusDto)
    {
        return Ok(await _service.UpdateStatus(updateStatusDto));
    }
}
