using AutoMapper;
using FreshBack.Application.Dtos.Roles;
using FreshBack.Domain.Models.Roles;

namespace FreshBack.Application.AutoMapper.Roles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
    }
}
