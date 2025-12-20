namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

public class GetUsersResponse
{
    public int Total { get; set; }
    public IEnumerable<GetUserResponse> Users { get; set; } = default!;
}
