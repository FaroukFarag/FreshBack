namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Schedules;

public class ScheduleDataRequest
{
    public Dictionary<string, object> Holiday { get; set; } = default!;
    public List<TimeRangeRequest> Monday { get; set; } = default!;
    public List<TimeRangeRequest> Tuesday { get; set; } = default!;
    public List<TimeRangeRequest> Wednesday { get; set; } = default!;
    public List<TimeRangeRequest> Thursday { get; set; } = default!;
    public List<TimeRangeRequest> Friday { get; set; } = default!;
    public List<TimeRangeRequest> Saturday { get; set; } = default!;
    public List<TimeRangeRequest> Sunday { get; set; } = default!;
}
