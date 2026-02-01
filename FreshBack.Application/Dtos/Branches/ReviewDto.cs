using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Dtos.Orders;
using FreshBack.Application.Dtos.Settings.Users;

namespace FreshBack.Application.Dtos.Branches;

public class ReviewDto : BaseImageModelDto<int>
{
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public int MerchantId { get; set; }
    public int BranchId { get; set; }
    public int OrderId { get; set; }

    public UserDto User { get; set; } = default!;
    public MerchantDto Merchant { get; set; } = default!;
    public BranchDto Branch { get; set; } = default!;
    public OrderDto Order { get; set; } = default!;
}
