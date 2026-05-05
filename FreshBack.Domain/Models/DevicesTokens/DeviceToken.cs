using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Customers;

namespace FreshBack.Domain.Models.DevicesTokens;

public class DeviceToken : BaseAuditModel<int>
{
    public string Token { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public int CustomerId { get; set; } = default!;

    public Customer Customer { get; set; } = default!;
}
