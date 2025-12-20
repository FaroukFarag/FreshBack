using AutoMapper;
using FreshBack.Application.Dtos.Roles;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Roles;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Roles;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Roles;
using Microsoft.AspNetCore.Identity;

namespace FreshBack.Application.Services.Roles;

public class RoleService(
    IRoleRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    RoleManager<Role> roleManager) :
    BaseService<RoleDto, RoleDto, RoleDto, RoleDto, Role, int>(repository,
        unitOfWork, mapper), IRoleService
{
    private readonly IMapper _mapper = mapper;
    private readonly RoleManager<Role> _roleManager = roleManager;

    public override async Task<ResultDto<RoleDto>> CreateAsync(RoleDto roleDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Role",
            action: async () =>
            {
                var role = _mapper.Map<Role>(roleDto);
                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Role creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                return roleDto;
            });
    }

    public override async Task<ResultDto<RoleDto>> UpdateAsync(RoleDto newRoleDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Update Role",
            action: async () =>
            {
                var role = _mapper.Map<Role>(newRoleDto);
                var result = await _roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Role update failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                return newRoleDto;
            });
    }
}
