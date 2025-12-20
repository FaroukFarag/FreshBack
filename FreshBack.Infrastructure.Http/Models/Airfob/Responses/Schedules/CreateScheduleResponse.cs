using System.Text.Json.Serialization;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Schedules;

public class CreateScheduleResponse
{
    public int Id { get; set; }
    public string Type { get; set; } = default!;
    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    public ScheduleDataResponse Data { get; set; } = default!;
}
