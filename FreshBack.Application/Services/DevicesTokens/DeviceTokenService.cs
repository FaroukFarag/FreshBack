using AutoMapper;
using FreshBack.Application.Dtos.DevicesTokens;
using FreshBack.Application.Interfaces.DevicesTokens;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.DevicesTokens;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.DevicesTokens;

namespace FreshBack.Application.Services.DevicesTokens;

public class DeviceTokenService(
    IDeviceTokenRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<
        CreateDeviceTokenDto,
        DeviceTokenDto,
        DeviceTokenDto,
        DeviceTokenDto,
        DeviceToken,
        int>(repository, unitOfWork, mapper), IDeviceTokenService
{
}
