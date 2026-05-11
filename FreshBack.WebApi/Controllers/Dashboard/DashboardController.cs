using FreshBack.Application.Interfaces.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FreshBack.WebApi.Controllers.Dashboard;

[Route("api/[controller]")]
[ApiController]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
{
    private readonly IDashboardService _dashboardService = dashboardService;

    [HttpGet("ActiveCustomers")]
    public async Task<IActionResult> ActiveCustomers()
    {
        return Ok(await _dashboardService.ActiveCustomersAsync());
    }

    [HttpGet("RegisteredMerchants")]
    public async Task<IActionResult> RegisteredMerchants()
    {
        return Ok(await _dashboardService.RegisteredMerchantsAsync());
    }

    [HttpGet("ActiveOrders")]
    public async Task<IActionResult> ActiveOrders()
    {
        return Ok(await _dashboardService.ActiveOrdersAsync());
    }

    [HttpGet("LatestOrders")]
    public async Task<IActionResult> LatestOrders()
    {
        return Ok(await _dashboardService.LatestOrdersAsync());
    }

    [HttpGet("TotalRevenue")]
    public async Task<IActionResult> TotalRevenue()
    {
        return Ok(await _dashboardService.TotalRevenueAsync());
    }

    [HttpGet("MerchantTotalRevenue")]
    public async Task<IActionResult> MerchantTotalRevenue()
    {
        int merchantId = GetMerchantId();

        return Ok(await _dashboardService.TotalRevenueAsync(merchantId));
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
        var merchantId = GetMerchantId();

        return Ok(await _dashboardService.SavedProductsWeightAsync(merchantId));
    }

    [HttpGet("SoldProducts")]
    public async Task<IActionResult> SoldProducts()
    {
        var merchantId = GetMerchantId();

        return Ok(await _dashboardService.SoldProductsAsync(merchantId));
    }

    [HttpGet("RemainingProducts")]
    public async Task<IActionResult> RemainingProducts()
    {
        var merchantId = GetMerchantId();

        return Ok(await _dashboardService.RemainingProductsAsync(merchantId));
    }

    [HttpGet("MonthlyGrowthPercentage")]
    public async Task<IActionResult> MonthlyGrowthPercentage()
    {
        return Ok(await _dashboardService.MonthlyGrowthPercentageAsync());
    }

    private int GetMerchantId()
    {
        return int.Parse(User.FindFirstValue("merchantId")!);
    }
}
