using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Auth;

public class AirfobLoginResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string Mfa { get; set; } = default!;
    public string Role { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    [JsonPropertyName("site_status")]
    public string SiteStatus { get; set; } = default!;

    [JsonPropertyName("distributor_id")]
    public int? DistributorId { get; set; }

    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = default!;

    [JsonPropertyName("app_id")]
    public int AppId { get; set; }

    [JsonPropertyName("websocket_server_url")]
    public string WebsocketServerUrl { get; set; } = default!;
}
