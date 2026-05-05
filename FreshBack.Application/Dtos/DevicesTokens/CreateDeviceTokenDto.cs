namespace FreshBack.Application.Dtos.DevicesTokens;

public class CreateDeviceTokenDto
{
    public string Token { get; set; } = default!;
    public bool IsActive { get; set; } = true;

}
