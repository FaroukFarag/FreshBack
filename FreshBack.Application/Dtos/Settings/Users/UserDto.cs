using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Domain.Enums.Settings.Users;

namespace FreshBack.Application.Dtos.Settings.Users;

public class UserDto : BaseModelDto<int>
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Password { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public UserStatus Status { get; set; }
    public int RoleId { get; set; }
    public int? MerchantId { get; set; }
}
