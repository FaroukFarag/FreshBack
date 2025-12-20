namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.FloorLevels;

public class CreateFloorLevelsRequest
{
    public IEnumerable<CreateFloorLevelRequest> Levels { get; set; } = default!;
}
