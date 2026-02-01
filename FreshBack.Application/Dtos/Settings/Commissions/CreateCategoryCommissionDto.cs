using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Settings.Commissions;

public class CreateCategoryCommissionDto : BaseModelDto<int>
{
    public int CommissionId { get; set; }
    public int CategoryId { get; set; }
    public decimal PercentageOfTotal { get; set; }
}
