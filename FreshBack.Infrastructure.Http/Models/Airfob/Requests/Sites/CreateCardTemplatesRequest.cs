namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Sites;

public class CreateCardTemplatesRequest
{
    public List<CreateCardTemplateRequest> Templates { get; set; } = default!;
}
