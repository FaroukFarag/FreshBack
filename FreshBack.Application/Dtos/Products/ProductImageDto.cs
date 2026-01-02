using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Products;

public class ProductImageDto : BaseImageModelDto<int>
{
    public int ProductId { get; set; }

    public ProductDto Product { get; set; } = default!;
}
