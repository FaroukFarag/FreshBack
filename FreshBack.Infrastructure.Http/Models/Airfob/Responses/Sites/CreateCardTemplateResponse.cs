using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;

public class CreateCardTemplateResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }
    public object Properties { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Contents { get; set; } = default!;

    [JsonPropertyName("upload_url")]
    public string UploadUrl { get; set; } = default!;
}
