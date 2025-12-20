using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Schedules;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

public class ScheduleDataResponse
{
    public Dictionary<string, object> Holiday { get; set; } = default!;
    public IEnumerable<TimeRangeRequest> Monday { get; set; } = default!;
    public IEnumerable<TimeRangeRequest> Tuesday { get; set; } = default!;
    public IEnumerable<TimeRangeRequest> Wednesday { get; set; } = default!;
    public IEnumerable<TimeRangeRequest> Thursday { get; set; } = default!;
    public IEnumerable<TimeRangeRequest> Friday { get; set; } = default!;
    public IEnumerable<TimeRangeRequest> Saturday { get; set; } = default!;
    public IEnumerable<TimeRangeRequest> Sunday { get; set; } = default!;
}
