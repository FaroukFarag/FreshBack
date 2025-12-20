namespace FreshBack.Application.Dtos.Users;

public class ResetPasswordDto
{
    public string UserName { get; set; } = default!;
    public string OldPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
