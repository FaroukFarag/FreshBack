using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Sites;

public class CreateSiteRequest
{
    public string Name { get; set; } = default!;

    [JsonPropertyName("parent_id")]
    public int ParentId { get; set; }

    [JsonPropertyName("distributor_id")]
    public int DistributorId { get; set; }

    [JsonPropertyName("mobile_id")]
    public int MobileId { get; set; }

    public string Address { get; set; } = default!;

    public string Status { get; set; } = default!;

    public string Country { get; set; } = default!;

    public string TimeZone { get; set; } = default!;

    public object Properties = default!;
}
