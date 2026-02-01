using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Categories;

namespace FreshBack.Domain.Models.Settings.Commissions;

public class CategoryCommission : BaseModel<int>
{
    public int CommissionId { get; set; }
    public int CategoryId { get; set; }
    public decimal PercentageOfTotal { get; set; }

    public Commission Commission { get; set; } = default!;
    public Category Category { get; set; } = default!;
}
