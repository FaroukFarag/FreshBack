using FreshBack.Application.Dtos.Customers;

namespace FreshBack.Application.Dtos.OtpCodes;

public class OtpVerifiedDto
{
    public string Token { get; set; } = default!;
    public CustomerDto? Customer { get; set; }
}
