using AutoMapper;
using FreshBack.Application.Dtos.OtpCodes;
using FreshBack.Domain.Models.OtpCodes;

namespace FreshBack.Application.AutoMapper.OtpCodes;

public class OtpCodeProfile : Profile
{
    public OtpCodeProfile()
    {
        CreateMap<OtpCode, OtpCodeDto>().ReverseMap();
    }
}
