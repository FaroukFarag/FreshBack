using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Settings.Users;

namespace FreshBack.Domain.Models.Merchants;

public class Review : BaseModel<int>
{
    public string Comment { get; set; } = default!;
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public int MerchantId { get; set; }

    public User User { get; set; } = default!;
    public Merchant Merchant { get; set; } = default!;
}
