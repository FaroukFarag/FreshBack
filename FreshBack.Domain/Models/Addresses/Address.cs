using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Customers;

namespace FreshBack.Domain.Models.Addresses;

public class Address : BaseModel<int>
{
    public string Country { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Area { get; set; } = default!;
    public string Street { get; set; } = default!;
    public int BuildingNo { get; set; }
    public int? FlatNo { get; set; }
    public string? Landmark { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = default!;
}
