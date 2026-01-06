using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Products;

namespace FreshBack.Application.Dtos.Carts;

public class CartItemDto : BaseModelDto<int>
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public CartDto Cart { get; set; } = default!;
    public ProductDto Product { get; set; } = default!;
}
