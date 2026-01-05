using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Merchants;

namespace FreshBack.Application.Dtos.Categories;

public class CategoryDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;

    public IEnumerable<MerchantDto>? Merchants { get; set; }
}
