using FreshBack.Application.Dtos.Settings.Commissions;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Settings.Commissions;

namespace FreshBack.Application.Interfaces.Settings.Commissions;

public interface ICommissionService : IBaseService<
    CreateCommissionDto,
    CommissionDto,
    CommissionDto,
    CreateCommissionDto,
    Commission,
    int>
{
    Task<ResultDto<decimal>> CalculateCommissionAsync(
        int categoryId, decimal orderTotal);
}
