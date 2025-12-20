using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

public class UserGroupResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    [JsonPropertyName("parent_id")]
    public int? ParentId { get; set; }

    public object Properties { get; set; } = default!;
}
