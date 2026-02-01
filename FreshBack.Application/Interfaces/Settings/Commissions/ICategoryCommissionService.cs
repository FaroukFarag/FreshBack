using FreshBack.Application.Dtos.Settings.Commissions;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Settings.Commissions;

namespace FreshBack.Application.Interfaces.Settings.Commissions;

public interface ICategoryCommissionService : IBaseService<
    CreateCategoryCommissionDto,
    CategoryCommissionDto,
    CategoryCommissionDto,
    CategoryCommissionDto,
    CategoryCommission,
    int>
{
}
