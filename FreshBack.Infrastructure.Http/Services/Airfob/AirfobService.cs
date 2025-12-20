using AccessControlSystem.Infrastructure.Http.Clients.Airfob;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.AccessLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.FloorLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Schedules;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Sites;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.AccessLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Accounts;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.FloorLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Schedules;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

namespace AccessControlSystem.Infrastructure.Http.Services.Airfob;

public class AirfobService(AirfobClient client) : IAirfobService
{
    private readonly AirfobClient _client = client;

    public async Task<IEnumerable<CreateScheduleResponse>> CreateSitesAsync(CreateSitesRequest request)
    {
        return await _client.PostAsync<CreateSitesRequest, IEnumerable<CreateScheduleResponse>>("v1/sites", request);
    }

    public async Task<IEnumerable<CreateScheduleResponse>> CreateSchedulesAsync(CreateSchedulesRequest request)
    {
        return await _client.PostAsync<CreateSchedulesRequest, IEnumerable<CreateScheduleResponse>>("v1/schedules", request);
    }

    public async Task<IEnumerable<CreateAccessLevelResponse>> CreateAccessLevelsAsync(CreateAccessLevelsRequest request)
    {
        return await _client.PostAsync<CreateAccessLevelsRequest, IEnumerable<CreateAccessLevelResponse>>("v1/access_levels", request);
    }

    public async Task<IEnumerable<CreateFloorLevelResponse>> CreateFloorLevelsAsync(CreateFloorLevelsRequest request)
    {
        return await _client.PostAsync<CreateFloorLevelsRequest, IEnumerable<CreateFloorLevelResponse>>("v1/floor_levels", request);
    }

    public async Task<IEnumerable<CreateUserResponse>> CreateUsersAsync(CreateUsersRequest request)
    {
        return await _client.PostAsync<CreateUsersRequest, IEnumerable<CreateUserResponse>>("v1/users", request);
    }

    public async Task<IEnumerable<CreateUserGroupResponse>> CreateUserGroupsAsync(CreateUserGroupsRequest request)
    {
        return await _client.PostAsync<CreateUserGroupsRequest, IEnumerable<CreateUserGroupResponse>>("v1/users/groups", request);
    }

    public async Task<GetSitesResponse> GetSitesAsync()
    {
        return await _client.GetAsync<GetSitesResponse>("v1/sites");
    }

    public async Task<IEnumerable<int>> GetSubSitesIdsAsync()
    {
        return await _client.GetAsync<IEnumerable<int>>("v1/sites/sub_ids");
    }

    public async Task<GetSelfAccountsResponse> GetSelfAccountsAsync()
    {
        return await _client.GetAsync<GetSelfAccountsResponse>("v1/accounts/self");
    }

    public async Task<GetSchedulesResponse> GetSchedulesAsync()
    {
        return await _client.GetAsync<GetSchedulesResponse>("v1/schedules");
    }

    public async Task<GetAccessLevelsResponse> GetAccessLevelsAsync()
    {
        return await _client.GetAsync<GetAccessLevelsResponse>("v1/access_levels");
    }

    public async Task<GetFloorLevelsResponse> GetFloorLevelsAsync()
    {
        return await _client.GetAsync<GetFloorLevelsResponse>("v1/floor_levels");
    }

    public async Task<GetUsersResponse> GetUsersAsync()
    {
        return await _client.GetAsync<GetUsersResponse>("v1/users");
    }

    public async Task<GetUserGroupsResponse> GetUserGroupsAsync()
    {
        return await _client.GetAsync<GetUserGroupsResponse>("v1/users/groups");
    }

    public async Task<IEnumerable<AssignUserGroupMemberResponse>> AssignUserGroupMembersAsync(AssignUserGroupMembersRequest request)
    {
        return await _client.PostAsync<AssignUserGroupMembersRequest, IEnumerable<AssignUserGroupMemberResponse>>("v1/users/groups/members", request);
    }

    public async Task<GetCardTemplatesResponse> GetCardTemplatesAsync()
    {
        return await _client.GetAsync<GetCardTemplatesResponse>("v1/sites/card_templates");
    }

    public async Task<IEnumerable<CreateCardTemplateResponse>> CreateCardTemplatesAsync(CreateCardTemplatesRequest request)
    {
        return await _client.PostAsync<CreateCardTemplatesRequest, IEnumerable<CreateCardTemplateResponse>>("v1/sites/card_templates", request);
    }

    public async Task<IEnumerable<GetRfCardResponse>> GetRfCardsAsync(int siteId)
    {
        return await _client.GetAsync<IEnumerable<GetRfCardResponse>>($"v1/sites/{siteId}/rfcards");
    }

    public async Task<IEnumerable<GetMessageTemplateResponse>> GetMessageTemplatesAsync(int siteId)
    {
        return await _client.GetAsync<IEnumerable<GetMessageTemplateResponse>>($"v1/sites/{siteId}/message_templates");
    }

    public async Task<IEnumerable<AssignRfCardResponse>> AssignRfCardsAsync(AssignRfCardsRequest request)
    {
        return await _client.PostAsync<AssignRfCardsRequest, IEnumerable<AssignRfCardResponse>>($"v1/sites/{request.SiteId}/rfcards", request);
    }

    public async Task<IEnumerable<CreateMessageTemplateResponse>> CreateMessageTemplatesAsync(CreateMessageTemplatesRequest request)
    {
        return await _client.PostAsync<CreateMessageTemplatesRequest, IEnumerable<CreateMessageTemplateResponse>>($"v1/sites/{request.SiteId}/message_templates", request);
    }
}
