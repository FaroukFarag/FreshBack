using FreshBack.Domain.Enums.Settings.Users;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.UsersRoles;
using Microsoft.AspNetCore.Identity;

namespace FreshBack.Domain.Models.Settings.Users;

public class User : IdentityUser<int>
{
    public UserStatus Status { get; set; }
    public int? MerchantId { get; set; }

    public Merchant Merchant { get; set; } = default!;
    public IEnumerable<UserRole> UserRoles { get; set; } = default!;
}
