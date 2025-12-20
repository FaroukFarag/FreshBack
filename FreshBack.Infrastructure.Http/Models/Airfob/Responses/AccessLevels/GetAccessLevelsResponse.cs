namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.AccessLevels;

public class GetAccessLevelsResponse
{
    public int Total { get; set; }
    public IEnumerable<GetAccessLevelResponse> AccessLevels { get; set; } = default!;
}
