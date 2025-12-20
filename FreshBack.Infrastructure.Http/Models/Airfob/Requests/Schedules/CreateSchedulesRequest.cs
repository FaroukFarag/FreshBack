namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Schedules;

public class CreateSchedulesRequest
{
    public IEnumerable<CreateScheduleRequest> Schedules { get; set; } = default!;
}
