using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.OtpCodes;

public class OtpCodeDto : BaseModelDto<int>
{
    public string PhoneNumber { get; set; } = default!;
    public string Otp { get; set; } = default!;
}
