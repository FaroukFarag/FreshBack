using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Domain.Enums.Merchants;

namespace FreshBack.Application.Dtos.Merchants;

public class CreateMerchantDto : BaseImageModelDto<int>
{
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string DescriptionEn { get; set; } = default!;
    public string Story { get; set; } = default!;
    public string StoryEn { get; set; } = default!;
    public int CategoryId { get; set; }
    public string Username { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Password { get; set; } = default!;
    public MerchantStatus Status { get; set; }
}
