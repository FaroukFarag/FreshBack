using Microsoft.AspNetCore.Identity;

namespace FreshBack.Domain.Models.Users;

public class User : IdentityUser<int>
{
    public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; } = default!;
}
