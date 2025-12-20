using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class UserAccessLevelRequest
{
    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("user_id")]
    public int? UserId { get; set; }

    [JsonPropertyName("access_level_id")]
    public int AccessLevelId { get; set; }

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }
}
