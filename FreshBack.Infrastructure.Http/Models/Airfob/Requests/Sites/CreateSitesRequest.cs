namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Sites;

public class CreateSitesRequest
{
    public IEnumerable<CreateSiteRequest> Sites { get; set; } = default!;
}
