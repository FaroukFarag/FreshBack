namespace FreshBack.Application.Dtos.Settings.Users;

public class LoggedInDto
{
    public int UserId { get; set; }
    public int? RoleId { get; set; }

    public string Token { get; set; } = default!;
}
