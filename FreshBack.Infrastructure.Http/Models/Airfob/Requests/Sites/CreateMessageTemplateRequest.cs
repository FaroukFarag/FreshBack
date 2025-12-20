using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Sites;

public class CreateMessageTemplateRequest
{
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Format { get; set; } = default!;

    [JsonPropertyName("language_code")]
    public string LanguageCode { get; set; } = default!;
    public string Title { get; set; } = default!;

    [JsonPropertyName("msg_template")]
    public string TemplateContent { get; set; } = default!;
    public object Properties { get; set; } = default!;
}
