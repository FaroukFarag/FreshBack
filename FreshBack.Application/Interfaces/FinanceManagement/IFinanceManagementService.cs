using FreshBack.Application.Dtos.FinanceManagement;
using FreshBack.Application.Dtos.Shared;

namespace FreshBack.Application.Interfaces.FinanceManagement;

public interface IFinanceManagementService
{
    Task<ResultDto<TotalRevenuesDto>> GetTotalRevenues();
    Task<ResultDto<OrderAverageValueDto>> GetOrderAverageValue();
    Task<ResultDto<IEnumerable<TotalAreaRevenuesDto>>> GetTotalRevenuesByArea(int? month);
    Task<ResultDto<IEnumerable<TotalMerchantRevenuesDto>>>
        GetTotalRevenuesByMerchant();
    Task<ResultDto<TotalCommissionsDto>> GetTotalCommissions();
}
