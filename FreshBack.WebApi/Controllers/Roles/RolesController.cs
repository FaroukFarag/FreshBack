using FreshBack.Application.Dtos.Roles;
using FreshBack.Application.Interfaces.Roles;
using FreshBack.Domain.Models.Roles;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Roles;

[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService service) :
    BaseController<IRoleService, RoleDto, RoleDto, RoleDto, RoleDto, Role,
        int>(service)
{
}

