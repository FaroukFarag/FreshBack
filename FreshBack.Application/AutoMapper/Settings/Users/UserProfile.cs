using AutoMapper;
using FreshBack.Application.Dtos.Settings.Users;
using FreshBack.Domain.Models.Settings.Users;

namespace FreshBack.Application.AutoMapper.Settings.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}
