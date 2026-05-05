using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Domain.Enums.Merchants;

namespace FreshBack.Application.Dtos.Merchants;

public class UpdateStatusDto : BaseModelDto<int>
{
    public MerchantStatus Status { get; set; } = default!;
}
