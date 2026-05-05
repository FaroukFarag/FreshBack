using AutoMapper;
using FreshBack.Application.Dtos.DevicesTokens;
using FreshBack.Domain.Models.DevicesTokens;

namespace FreshBack.Application.AutoMapper.DevicesTokens;

public class DeviceTokenProfile : Profile
{
    public DeviceTokenProfile()
    {
        CreateMap<DeviceToken, CreateDeviceTokenDto>().ReverseMap();

        CreateMap<DeviceToken, DeviceTokenDto>().ReverseMap();
    }
}
