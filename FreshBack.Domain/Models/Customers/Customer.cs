using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Addresses;
using FreshBack.Domain.Models.BranchesFavorites;
using FreshBack.Domain.Models.Carts;
using FreshBack.Domain.Models.Orders;

namespace FreshBack.Domain.Models.Customers;

public class Customer : BaseModel<int>
{
    public string PhoneNumber { get; set; } = default!;
    public string? Name { get; set; }
    public string? NameEn { get; set; }
    public string? Email { get; set; }

    public IEnumerable<Address> Addresses { get; set; } = default!;
    public IEnumerable<Cart> Carts { get; set; } = default!;
    public IEnumerable<Order> Orders { get; set; } = default!;
    public IEnumerable<CustomerBranchFavorite> CustomersBranchesFavorite { get; set; } = default!;
}
