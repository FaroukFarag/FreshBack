using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Categories;

namespace FreshBack.Application.Dtos.Settings.Commissions;

public class CategoryCommissionDto : BaseModelDto<int>
{
    public int CommissionId { get; set; }
    public int CategoryId { get; set; }
    public decimal PercentageOfTotal { get; set; }

    public CommissionDto Commission { get; set; } = default!;
    public CategoryDto Category { get; set; } = default!;
}
