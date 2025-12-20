namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class CreateUsersRequest
{
    public List<CreateUserRequest> Users { get; set; } = default!;
}
