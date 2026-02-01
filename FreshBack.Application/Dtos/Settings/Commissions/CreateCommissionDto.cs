using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Domain.Enums.Settings.Commissions;

namespace FreshBack.Application.Dtos.Settings.Commissions;

public class CreateCommissionDto : BaseModelDto<int>
{
    public CommissionType Type { get; set; }
    public decimal? FixedAmount { get; set; }
    public IEnumerable<CreateCategoryCommissionDto>? CategoryCommissions { get; set; }
}
