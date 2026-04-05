using FreshBack.Application.Interfaces.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Dashboard;

[Route("api/[controller]")]
[ApiController]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
{
    private readonly IDashboardService _dashboardService = dashboardService;

    [HttpGet("SavedProductsWeight")]
    public async Task<IActionResult> SavedProductsWeight()
    {
        return Ok(await _dashboardService.SavedProductsWeightAsync());
    }

    [HttpGet("MonthlyGrowthPercentage")]
    public async Task<IActionResult> MonthlyGrowthPercentage()
    {
        return Ok(await _dashboardService.MonthlyGrowthPercentageAsync());
    }
}
