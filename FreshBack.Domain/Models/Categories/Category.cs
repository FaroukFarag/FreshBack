using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Branches;

namespace FreshBack.Domain.Models.Categories;

public class Category : BaseModel<int>
{
    public string Name { get; set; } = default!;

    public IEnumerable<Branch> Branches { get; set; } = default!;
}
