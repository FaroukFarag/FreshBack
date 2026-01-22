using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Customers;

public class CreateCustomerDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string? Token { get; set; }
}
