using FreshBack.Application.Dtos.Roles;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Roles;

namespace FreshBack.Application.Interfaces.Roles;

public interface IRoleService : IBaseService<RoleDto, RoleDto, RoleDto, RoleDto,
    Role, int>
{
}
