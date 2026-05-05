using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Orders;

public class OrderConfirmedDto : BaseModelDto<int>
{
    public int MerchantId { get; set; }
    public string MerchantName { get; set; } = default!;
    public int BranchId { get; set; }
    public string BranchName { get; set; } = default!;
}
