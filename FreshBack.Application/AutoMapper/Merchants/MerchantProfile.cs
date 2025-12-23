using AutoMapper;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Domain.Models.Merchants;

namespace FreshBack.Application.AutoMapper.Merchants;

public class MerchantProfile : Profile
{
    public MerchantProfile()
    {
        CreateMap<Merchant, MerchantDto>().ReverseMap();
    }
}
