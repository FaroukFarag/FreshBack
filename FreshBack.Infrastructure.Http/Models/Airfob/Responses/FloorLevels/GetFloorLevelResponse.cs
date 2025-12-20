using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;
using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.FloorLevels;

public class GetFloorLevelResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    [JsonPropertyName("schedule_id")]
    public int ScheduleId { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime? StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime? EndDate { get; set; }
    public object Properties { get; set; } = default!;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<object> Floors { get; set; } = default!;
    public ScheduleDataResponse Schedule { get; set; } = default!;
}
