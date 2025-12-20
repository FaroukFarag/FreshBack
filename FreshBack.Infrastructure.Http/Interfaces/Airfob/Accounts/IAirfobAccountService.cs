using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Accounts;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Accounts;

public interface IAirfobAccountService
{
    Task<GetSelfAccountsResponse> GetSelfAccountsAsync();
}
