using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.OrdersPhotos;

public class CreateOrderPhotoDto : BaseImageModelDto<int>
{
    public int OrderId { get; set; }
    public int BranchId { get; set; }
}
