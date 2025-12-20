using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.AccessLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.AccessLevels;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.AccessLevels;

public interface IAirfobAccessLevelService
{
    Task<GetAccessLevelsResponse> GetAccessLevelsAsync();
    Task<IEnumerable<CreateAccessLevelResponse>> CreateAccessLevelsAsync(CreateAccessLevelsRequest request);
}
