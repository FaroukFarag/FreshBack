using FreshBack.Domain.Models.Abstraction;

namespace FreshBack.Domain.Models.Products;

public class ProductImage : BaseImageModel<int>
{
    public int ProductId { get; set; }

    public Product Product { get; set; } = default!;
}
