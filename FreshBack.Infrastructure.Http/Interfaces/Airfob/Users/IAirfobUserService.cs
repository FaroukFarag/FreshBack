using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Users;

public interface IAirfobUserService
{
    Task<GetUsersResponse> GetUsersAsync();
    Task<GetUserGroupsResponse> GetUserGroupsAsync();
    Task<IEnumerable<CreateUserResponse>> CreateUsersAsync(CreateUsersRequest request);
    Task<IEnumerable<CreateUserGroupResponse>> CreateUserGroupsAsync(CreateUserGroupsRequest request);
    Task<IEnumerable<AssignUserGroupMemberResponse>> AssignUserGroupMembersAsync(AssignUserGroupMembersRequest request);
}
