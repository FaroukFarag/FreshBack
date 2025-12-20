namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Schedules;

public class GetSchedulesResponse
{
    public int Total { get; set; }
    public IEnumerable<GetScheduleResponse> Schedules { get; set; } = default!;
}
