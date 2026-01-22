using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Orders;

namespace FreshBack.Domain.Models.OrdersPhotos;

public class OrderPhoto : BaseImageModel<int>
{
    public int OrderId { get; set; }
    public int BranchId { get; set; }

    public Order Order { get; set; } = default!;
    public Branch Branch { get; set; } = default!;
}
