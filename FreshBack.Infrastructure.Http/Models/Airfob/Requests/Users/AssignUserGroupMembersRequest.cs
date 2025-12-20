using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class AssignUserGroupMembersRequest
{
    [JsonPropertyName("user_ids")]
    public IEnumerable<int> UserIds { get; set; } = default!;

    [JsonPropertyName("group_ids")]
    public IEnumerable<int> GroupIds { get; set; } = default!;
}
