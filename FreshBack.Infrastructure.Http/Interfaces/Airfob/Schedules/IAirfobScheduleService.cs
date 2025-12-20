using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Schedules;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Schedules;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Schedules;

public interface IAirfobScheduleService
{
    Task<IEnumerable<CreateScheduleResponse>> CreateSchedulesAsync(CreateSchedulesRequest request);
    Task<GetSchedulesResponse> GetSchedulesAsync();
}
