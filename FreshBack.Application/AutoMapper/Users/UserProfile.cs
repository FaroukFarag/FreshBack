using AutoMapper;
using FreshBack.Application.Dtos.Users;
using FreshBack.Domain.Models.Users;

namespace FreshBack.Application.AutoMapper.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}
