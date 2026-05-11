using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Products;

public class GetProductImageDto : BaseImageModelDto<int>
{
    public int ProductId { get; set; }
}
