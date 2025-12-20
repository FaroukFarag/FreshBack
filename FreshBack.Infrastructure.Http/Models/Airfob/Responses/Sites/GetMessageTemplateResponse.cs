using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;

public class GetMessageTemplateResponse
{
    public int Id { get; set; }

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Format { get; set; } = default!;

    [JsonPropertyName("language_code")]
    public string LanguageCode { get; set; } = default!;
    public string Title { get; set; } = default!;

    [JsonPropertyName("app_id")]
    public int AppId { get; set; }

    [JsonPropertyName("msg_template")]
    public string MessageTemplate { get; set; } = default!;
    public string Sender { get; set; } = default!;
    public object Properties { get; set; } = default!;

    [JsonPropertyName("account_id")]
    public int AccountId { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = default!;

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; } = default!;

    [JsonPropertyName("card_type")]
    public string CardType { get; set; } = default!;

    [JsonPropertyName("account_name")]
    public string AccountName { get; set; } = default!;
}
