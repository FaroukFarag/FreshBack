using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Products;

namespace FreshBack.Domain.Models.Carts;

public class CartItem : BaseModel<int>
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public Cart Cart { get; set; } = default!;
    public Product Product { get; set; } = default!;
}
