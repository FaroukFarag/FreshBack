using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Schedules;

public class CreateScheduleRequest
{
    public string Type { get; set; } = default!;
    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    public ScheduleDataRequest Data { get; set; } = default!;
}
