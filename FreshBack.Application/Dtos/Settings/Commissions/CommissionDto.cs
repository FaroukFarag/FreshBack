using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Domain.Enums.Settings.Commissions;

namespace FreshBack.Application.Dtos.Settings.Commissions;

public class CommissionDto : BaseModelDto<int>
{
    public CommissionType Type { get; set; }
    public decimal? FixedAmount { get; set; }
    public IEnumerable<CategoryCommissionDto> CategoryCommissions { get; set; } = default!;
}
