using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.FloorLevels;

public class GetFloorLevelsResponse
{
    public int Total { get; set; }

    [JsonPropertyName("floor_levels")]
    public IEnumerable<GetFloorLevelResponse> FloorLevels { get; set; } = default!;

}
