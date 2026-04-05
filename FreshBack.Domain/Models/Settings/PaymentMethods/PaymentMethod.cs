using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Orders;

namespace FreshBack.Domain.Models.Settings.PaymentMethods;

public class PaymentMethod : BaseAuditModel<int>
{
    public string NameAr { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public bool IsActive { get; set; }

    public IEnumerable<Order> Orders { get; set; } = default!;
}
