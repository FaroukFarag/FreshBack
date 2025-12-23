using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Settings.Users;

namespace FreshBack.Application.Dtos.Merchants;

public class ReviewDto : BaseModelDto<int>
{
    public string Comment { get; set; } = default!;
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public int MerchantId { get; set; }

    public UserDto User { get; set; } = default!;
    public MerchantDto Merchant { get; set; } = default!;
}
