using AutoMapper;
using FreshBack.Application.AutoMapper.Resolvers;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Domain.Models.Merchants;

namespace FreshBack.Application.AutoMapper.Merchants;

public class MerchantProfile : Profile
{
    public MerchantProfile()
    {
        CreateMap<Merchant, CreateMerchantDto>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<ImagePathToUrlResolver<Merchant, CreateMerchantDto>>());

        CreateMap<CreateMerchantDto, Merchant>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<ImageUrlToPathResolver<CreateMerchantDto, Merchant>>());

        CreateMap<Merchant, MerchantDto>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<ImagePathToUrlResolver<Merchant, MerchantDto>>());

        CreateMap<MerchantDto, Merchant>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<ImageUrlToPathResolver<MerchantDto, Merchant>>());
    }
}
