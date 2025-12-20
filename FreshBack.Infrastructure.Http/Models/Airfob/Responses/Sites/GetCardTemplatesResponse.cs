namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;

public class GetCardTemplatesResponse
{
    public int Count { get; set; }

    public List<GetCardTemplateResponse> Rows { get; set; } = default!;
}
