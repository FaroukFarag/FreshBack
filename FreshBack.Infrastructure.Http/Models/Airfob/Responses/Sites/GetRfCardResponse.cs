using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;

public class GetRfCardResponse
{
    public int Id { get; set; }
    public string Data { get; set; } = default!;

    [JsonPropertyName("user_id")]
    public int? UserId { get; set; }

    [JsonPropertyName("device_id")]
    public int? DeviceId { get; set; }

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }
    public object Properties { get; set; } = default!;

    public string Status { get; set; } = default!;
}
