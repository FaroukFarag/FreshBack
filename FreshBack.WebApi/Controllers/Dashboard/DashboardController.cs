using FreshBack.Application.Interfaces.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FreshBack.WebApi.Controllers.Dashboard;

[Route("api/[controller]")]
[ApiController]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
{
    private readonly IDashboardService _dashboardService = dashboardService;

    [HttpGet("TotalRevenue")]
    public async Task<IActionResult> TotalRevenue()
    {
        return Ok(await _dashboardService.TotalRevenueAsync());
    }

    [HttpGet("OccupancyRate")]
    public async Task<IActionResult> OccupancyRate()
    {
        return Ok(await _dashboardService.OccupancyRateAsync());
    }

    [HttpGet("HourlySales")]
    public async Task<IActionResult> HourlySales()
    {
        return Ok(await _dashboardService.HourlySalesAsync());
    }

    [HttpGet("WeeklySales")]
    public async Task<IActionResult> WeeklySales()
    {
        return Ok(await _dashboardService.WeeklySalesAsync());
    }

    [HttpGet("SavedProductsWeight")]
    public async Task<IActionResult> SavedProductsWeight()
    {
        return Ok(await _dashboardService.SavedProductsWeightAsync());
    }

    [HttpGet("MerchantSavedProductsWeight")]
    public async Task<IActionResult> MerchantSavedProductsWeight()
    {
        var merchantId = int.Parse(User.FindFirstValue("merchantId")!);

        return Ok(await _dashboardService.SavedProductsWeightAsync(merchantId));
    }

    [HttpGet("SoldProducts")]
    public async Task<IActionResult> SoldProducts()
    {
        var merchantId = int.Parse(User.FindFirstValue("merchantId")!);

        return Ok(await _dashboardService.SoldProductsAsync(merchantId));
    }

    [HttpGet("RemainingProducts")]
    public async Task<IActionResult> RemainingProducts()
    {
        var merchantId = int.Parse(User.FindFirstValue("merchantId")!);

        return Ok(await _dashboardService.RemainingProductsAsync(merchantId));
    }

    [HttpGet("MonthlyGrowthPercentage")]
    public async Task<IActionResult> MonthlyGrowthPercentage()
    {
        return Ok(await _dashboardService.MonthlyGrowthPercentageAsync());
    }
}
