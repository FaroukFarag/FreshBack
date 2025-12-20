namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Accounts;

public class GetSelfAccountsResponse
{
    public IEnumerable<AccountResponse> Accounts { get; set; } = default!;
}
