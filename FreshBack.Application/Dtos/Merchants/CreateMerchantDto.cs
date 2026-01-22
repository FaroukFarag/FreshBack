using FreshBack.Domain.Enums.Merchants;

namespace FreshBack.Application.Dtos.Merchants;

public class CreateMerchantDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Story { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public MerchantStatus Status { get; set; }
    public int AreaId { get; set; }
}
