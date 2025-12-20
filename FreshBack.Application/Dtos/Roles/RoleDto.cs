using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Roles;

public class RoleDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
}
