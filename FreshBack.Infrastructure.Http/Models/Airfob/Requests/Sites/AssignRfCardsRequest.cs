namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Sites;

public class AssignRfCardsRequest
{
    public int SiteId { get; set; }
    public List<AssignRfCardRequest> Cards { get; set; } = default!;
}
