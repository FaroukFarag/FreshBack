using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Customers;

public class CreateCustomerDto : BaseModelDto<int>
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
    public string Token { get; set; } = default!;
}
