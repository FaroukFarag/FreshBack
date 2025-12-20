namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.AccessLevels;

public class CreateAccessLevelsRequest
{
    public IEnumerable<CreateAccessLevelRequest> Levels { get; set; } = default!;
}
