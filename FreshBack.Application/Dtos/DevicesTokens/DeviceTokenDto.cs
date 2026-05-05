using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Customers;

namespace FreshBack.Application.Dtos.DevicesTokens;

public class DeviceTokenDto : BaseModelDto<int>
{
    public string Token { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public int CustomerId { get; set; } = default!;

    public CustomerDto Customer { get; set; } = default!;
}
