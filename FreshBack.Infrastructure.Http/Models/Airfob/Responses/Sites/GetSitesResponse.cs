namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;

public class GetSitesResponse
{
    public int Total { get; set; }
    public IEnumerable<GetSiteResponse> Sites { get; set; } = default!;
}
