namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Auth;

public class AirfobLoginRequest
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}
