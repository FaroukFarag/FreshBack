using FreshBack.Domain.Models.Abstraction;

namespace FreshBack.Domain.Models.OtpCodes;

public class OtpCode : BaseModel<int>
{
    public string PhoneNumber { get; set; } = default!;
    public string CodeHash { get; set; } = default!;
    public DateTime ExpireAt { get; set; }
    public bool IsUsed { get; set; }
}

