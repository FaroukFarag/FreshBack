using FreshBack.Domain.Models.Roles;
using FreshBack.Domain.Models.Settings.Users;
using Microsoft.AspNetCore.Identity;

namespace FreshBack.Domain.Models.UsersRoles;

public class UserRole : IdentityUserRole<int>
{
    public User User { get; set; } = default!;
    public Role Role { get; set; } = default!;
}
