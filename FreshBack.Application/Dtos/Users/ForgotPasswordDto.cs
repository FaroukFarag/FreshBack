namespace FreshBack.Application.Dtos.Users;

public class ForgotPasswordDto
{
    public string UserName { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
