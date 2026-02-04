using FreshBack.Application.Interfaces.FinanceManagement;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.FinanceManagement;

[Route("api/[controller]")]
[ApiController]
public class FinanceManagementController(
    IFinanceManagementService service) : ControllerBase
{
    private readonly IFinanceManagementService _service = service;

    [HttpGet("GetTotalRevenues")]
    public async Task<IActionResult> GetTotalRevenues()
    {
        return Ok(await _service.GetTotalRevenues());
    }

    [HttpGet("GetOrderAverageValue")]
    public async Task<IActionResult> GetOrderAverageValue()
    {
        return Ok(await _service.GetOrderAverageValue());
    }

    [HttpGet("GetTotalRevenuesByArea")]
    public async Task<IActionResult> GetTotalRevenuesByArea(int? month = null)
    {
        return Ok(await _service.GetTotalRevenuesByArea(month));
    }

    [HttpGet("GetTotalRevenuesByMerchant")]
    public async Task<IActionResult> GetTotalRevenuesByMerchant()
    {
        return Ok(await _service.GetTotalRevenuesByMerchant());
    }

    [HttpGet("GetTotalCommissions")]
    public async Task<IActionResult> GetTotalCommissions()
    {
        return Ok(await _service.GetTotalCommissions());
    }
}
