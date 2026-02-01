using FreshBack.Application.Interfaces.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Statistics;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController(IStatisticsService service) : ControllerBase
{
    private readonly IStatisticsService _service = service;

    [HttpGet("GetTopTenMerchants")]
    public async Task<IActionResult> GetTopTenMerchants()
    {
        return Ok(await _service.GetTopTenMerchants());
    }

    [HttpGet("GetTopTenProducts")]
    public async Task<IActionResult> GetTopTenProducts()
    {
        return Ok(await _service.GetTopTenProducts());
    }
}
