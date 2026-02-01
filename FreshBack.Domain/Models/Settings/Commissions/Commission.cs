using FreshBack.Domain.Enums.Settings.Commissions;
using FreshBack.Domain.Models.Abstraction;

namespace FreshBack.Domain.Models.Settings.Commissions;

public class Commission : BaseModel<int>
{
    public CommissionType Type { get; set; }
    public decimal? FixedAmount { get; set; }
    public IEnumerable<CategoryCommission> CategoryCommissions { get; set; } = default!;
}
