using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Merchants;

namespace FreshBack.Domain.Models.Categories;

public class Category : BaseModel<int>
{
    public string Name { get; set; } = default!;

    public IEnumerable<Merchant> Merchants { get; set; } = default!;
}
