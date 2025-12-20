using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

public class UserAccessLevelResponse
{
    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    [JsonPropertyName("device_group_id")]
    public int? DeviceGroupId { get; set; }

    [JsonPropertyName("schedule_id")]
    public int ScheduleId { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }
    public object Properties { get; set; } = default!;
}
