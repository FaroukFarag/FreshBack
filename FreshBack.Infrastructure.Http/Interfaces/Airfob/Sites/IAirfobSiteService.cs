using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Sites;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Schedules;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Sites;

public interface IAirfobSiteService
{
    Task<GetSitesResponse> GetSitesAsync();
    Task<IEnumerable<int>> GetSubSitesIdsAsync();
    Task<GetCardTemplatesResponse> GetCardTemplatesAsync();
    Task<IEnumerable<GetRfCardResponse>> GetRfCardsAsync(int siteId);
    Task<IEnumerable<GetMessageTemplateResponse>> GetMessageTemplatesAsync(int siteId);
    Task<IEnumerable<CreateScheduleResponse>> CreateSitesAsync(CreateSitesRequest request);
    Task<IEnumerable<CreateCardTemplateResponse>> CreateCardTemplatesAsync(CreateCardTemplatesRequest request);
    Task<IEnumerable<AssignRfCardResponse>> AssignRfCardsAsync(AssignRfCardsRequest request);
    Task<IEnumerable<CreateMessageTemplateResponse>> CreateMessageTemplatesAsync(CreateMessageTemplatesRequest request);
}
