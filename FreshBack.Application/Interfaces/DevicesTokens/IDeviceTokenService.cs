using FreshBack.Application.Dtos.DevicesTokens;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.DevicesTokens;

namespace FreshBack.Application.Interfaces.DevicesTokens;

public interface IDeviceTokenService : IBaseService<
    CreateDeviceTokenDto,
    DeviceTokenDto,
    DeviceTokenDto,
    DeviceTokenDto,
    DeviceToken,
    int>
{
}
