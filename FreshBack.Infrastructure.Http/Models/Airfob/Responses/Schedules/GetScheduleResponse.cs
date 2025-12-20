using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;
using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Schedules;

public class GetScheduleResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public int? SiteId { get; set; }
    public string Type { get; set; } = default!;
    public ScheduleDataResponse Data { get; set; } = default!;
    public string Description { get; set; } = default!;
}
