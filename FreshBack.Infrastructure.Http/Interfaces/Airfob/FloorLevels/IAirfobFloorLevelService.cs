using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.FloorLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.FloorLevels;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.FloorLevels;

public interface IAirfobFloorLevelService
{
    Task<GetFloorLevelsResponse> GetFloorLevelsAsync();
    Task<IEnumerable<CreateFloorLevelResponse>> CreateFloorLevelsAsync(CreateFloorLevelsRequest request);
}
