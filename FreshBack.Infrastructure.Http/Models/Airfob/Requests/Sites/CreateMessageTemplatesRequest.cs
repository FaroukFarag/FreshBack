namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Sites;

public class CreateMessageTemplatesRequest
{
    public int SiteId { get; set; }
    public IEnumerable<CreateMessageTemplateRequest> Templates { get; set; } = default!;
}
