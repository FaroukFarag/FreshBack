using FreshBack.Application.Dtos.Dashboard;
using FreshBack.Application.Dtos.Shared;

namespace FreshBack.Application.Interfaces.Dashboard;

public interface IDashboardService
{
    Task<ResultDto<ActiveCustomersDto>> ActiveCustomersAsync();
    Task<ResultDto<RegisteredMerchantsDto>> RegisteredMerchantsAsync();
    Task<ResultDto<RevenueDto>> TotalRevenueAsync(int? merchantId = null);
    Task<ResultDto<ActiveOrdersDto>> ActiveOrdersAsync();
    Task<ResultDto<IEnumerable<LatestOrderDto>>> LatestOrdersAsync(int count = 10);
    Task<ResultDto<OccupancyRateDto>> OccupancyRateAsync();
    Task<ResultDto<HourlySalesDto>> HourlySalesAsync();
    Task<ResultDto<WeeklySalesDto>> WeeklySalesAsync();
    Task<ResultDto<decimal>> SavedProductsWeightAsync(int? merchantId = null);
    Task<ResultDto<decimal>> SoldProductsAsync(int? merchantId = null);
    Task<ResultDto<decimal>> RemainingProductsAsync(int? merchantId = null);
    Task<ResultDto<decimal>> MonthlyGrowthPercentageAsync();
}
