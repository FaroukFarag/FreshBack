using FreshBack.Domain.Models.UsersRoles;
using Microsoft.AspNetCore.Identity;

namespace FreshBack.Domain.Models.Roles;

public class Role : IdentityRole<int>
{
    public IEnumerable<UserRole> RoleUsers { get; set; } = default!;
}
