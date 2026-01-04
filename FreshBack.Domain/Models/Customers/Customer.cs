using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Orders;

namespace FreshBack.Domain.Models.Customers;

public class Customer : BaseModel<int>
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Area { get; set; } = default!;
    public string Street { get; set; } = default!;
    public int BuildingNo { get; set; }
    public int? FlatNo { get; set; }
    public string? Landmark { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }

    public IEnumerable<Order> Orders { get; set; } = default!;
}
