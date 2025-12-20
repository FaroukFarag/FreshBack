using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

public class GetUserGroupResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    [JsonPropertyName("parent_id")]
    public int? ParentId { get; set; }

    public Dictionary<string, object> Properties { get; set; } = default!;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("total_user")]
    public int TotalUser { get; set; }

    [JsonPropertyName("total_activate_user")]
    public int TotalActivateUser { get; set; }
}
