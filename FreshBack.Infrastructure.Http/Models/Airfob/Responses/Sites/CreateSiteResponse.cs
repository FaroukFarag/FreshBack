using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;

public class CreateSiteResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    [JsonPropertyName("parent_id")]
    public int ParentId { get; set; }

    [JsonPropertyName("distributor_id")]
    public int DistributorId { get; set; }

    [JsonPropertyName("mobile_id")]
    public string MobileId { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string Status { get; set; } = default!;

    public string Country { get; set; } = default!;

    public string TimeZone { get; set; } = default!;

    public string Properties { get; set; } = default!;

    [JsonPropertyName("provider_type")]
    public string ProviderType { get; set; } = default!;
}
