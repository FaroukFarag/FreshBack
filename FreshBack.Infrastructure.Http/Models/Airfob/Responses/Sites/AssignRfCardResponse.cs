using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;

public class AssignRfCardResponse
{
    public int Id { get; set; }
    public string Data { get; set; } = default!;

    public object Properties { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }
}
