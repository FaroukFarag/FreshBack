using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Categories;

public class CreateCategoryDto : BaseImageModelDto<int>
{
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
}
