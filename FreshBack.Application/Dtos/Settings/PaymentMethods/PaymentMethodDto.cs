using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Settings.PaymentMethods;

public class PaymentMethodDto : BaseModelDto<int>
{
    public string NameAr { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public bool IsActive { get; set; }
}
