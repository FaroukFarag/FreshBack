using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Dtos.Categories;
using FreshBack.Domain.Enums.Merchants;

namespace FreshBack.Application.Dtos.Merchants;

public class MerchantDto : BaseImageModelDto<int>
{
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string DescriptionEn { get; set; } = default!;
    public string Story { get; set; } = default!;
    public string StoryEn { get; set; } = default!;
    public int CategoryId { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public MerchantStatus Status { get; set; }

    public CategoryDto Category { get; set; } = default!;
    public IEnumerable<ReviewDto> Reviews { get; set; } = default!;
}
