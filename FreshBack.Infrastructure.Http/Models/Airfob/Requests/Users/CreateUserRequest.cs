using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class CreateUserRequest
{
    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    [JsonPropertyName("user_key")]
    public string UserKey { get; set; } = default!;

    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }
    public string Email { get; set; } = default!;
    public string Mobile { get; set; } = default!;

    [JsonPropertyName("used_mobile_card")]
    public bool UsedMobileCard { get; set; }

    [JsonPropertyName("mobile_card_id")]
    public int? MobileCardId { get; set; }

    [JsonPropertyName("used_web_qr_card")]
    public bool UsedWebQrCard { get; set; }

    [JsonPropertyName("used_web_link_card")]
    public bool UsedWebLinkCard { get; set; }

    [JsonPropertyName("rf_card_id")]
    public int? RfCardId { get; set; }

    [JsonPropertyName("access_levels")]
    public List<UserAccessLevelRequest> AccessLevels { get; set; } = default!;

    [JsonPropertyName("floor_levels")]
    public List<UserFloorLevelRequest> FloorLevels { get; set; } = default!;

    [JsonPropertyName("group_ids")]
    public List<int> GroupIds { get; set; } = default!;
    public object Properties { get; set; } = default!;
    public string Type { get; set; } = default!;

    [JsonPropertyName("used_mobile_qr_card")]
    public bool UsedMobileQrCard { get; set; }
}
