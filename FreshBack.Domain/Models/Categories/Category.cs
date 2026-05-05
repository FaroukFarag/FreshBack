using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.Settings.Commissions;

namespace FreshBack.Domain.Models.Categories;

public class Category : BaseImageAuditModel<int>
{
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;

    public CategoryCommission CategoryCommission { get; set; } = default!;
    public IEnumerable<Merchant> Merchants { get; set; } = default!;
}
