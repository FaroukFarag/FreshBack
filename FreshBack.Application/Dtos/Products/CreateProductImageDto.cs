using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Products;

public class CreateProductImageDto : BaseImageModelDto<int>
{
    public int ProductId { get; set; }
}
