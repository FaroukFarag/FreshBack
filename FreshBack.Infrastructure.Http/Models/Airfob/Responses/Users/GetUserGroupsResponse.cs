using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

public class GetUserGroupsResponse
{
    public int Total { get; set; }

    [JsonPropertyName("groups")]
    public IEnumerable<GetUserGroupResponse> Groups { get; set; } = default!;
}
