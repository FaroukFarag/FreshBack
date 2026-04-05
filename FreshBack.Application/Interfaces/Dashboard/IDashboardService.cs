using FreshBack.Application.Dtos.Shared;

namespace FreshBack.Application.Interfaces.Dashboard;

public interface IDashboardService
{
    Task<ResultDto<decimal>> SavedProductsWeightAsync();
    Task<ResultDto<decimal>> MonthlyGrowthPercentageAsync();
}
