using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;

public class CreateMessageTemplateResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Format { get; set; } = default!;

    [JsonPropertyName("language_code")]
    public string LanguageCode { get; set; } = default!;
    public string Title { get; set; } = default!;

    [JsonPropertyName("msg_template")]
    public string MessageTemplate { get; set; } = default!;
    public object Properties { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    [JsonPropertyName("app_id")]
    public int AppId { get; set; }

    [JsonPropertyName("account_id")]
    public int AccountId { get; set; } = default!;
    public string Sender { get; set; } = default!;
}
