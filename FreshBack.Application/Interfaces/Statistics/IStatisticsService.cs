using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Dtos.Statistics;

namespace FreshBack.Application.Interfaces.Statistics;

public interface IStatisticsService
{
    Task<ResultDto<IEnumerable<TopMerchantDto>>> GetTopTenMerchants();
    Task<ResultDto<IEnumerable<TopProductDto>>> GetTopTenProducts();
}
