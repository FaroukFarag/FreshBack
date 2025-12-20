using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;

public class GetCardTemplateResponse
{
    public int Id { get; set; }

    [JsonPropertyName("site_id")]
    public int? SiteId { get; set; }
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Contents { get; set; } = default!;
    public object Properties { get; set; } = default!;
}
