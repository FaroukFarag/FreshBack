using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Dtos.Orders;

namespace FreshBack.Application.Dtos.OrdersPhotos;

public class OrderPhotoDto : BaseImageModelDto<int>
{
    public int OrderId { get; set; }
    public int BranchId { get; set; }

    public OrderDto Order { get; set; } = default!;
    public BranchDto Branch { get; set; } = default!;
}
