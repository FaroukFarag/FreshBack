using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Customers;

namespace FreshBack.Domain.Carts;

public class Cart : BaseModel<int>
{
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = default!;
    public IEnumerable<CartItem> CartItems { get; set; } = default!;
}
