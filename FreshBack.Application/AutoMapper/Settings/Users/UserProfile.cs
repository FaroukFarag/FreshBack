using AutoMapper;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Dtos.Settings.Users;
using FreshBack.Domain.Enums.Roles;
using FreshBack.Domain.Models.Settings.Users;

namespace FreshBack.Application.AutoMapper.Settings.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();

        CreateMap<CreateMerchantDto, UserDto>()
            .ForMember(des => des.RoleId, opt => opt.MapFrom(src => (int)RoleNames.Merchant));
    }
}
