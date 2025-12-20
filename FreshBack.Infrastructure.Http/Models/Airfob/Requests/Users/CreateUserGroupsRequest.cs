namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class CreateUserGroupsRequest
{
    public IEnumerable<CreateUserGroupRequest> Groups { get; set; } = default!;
}
