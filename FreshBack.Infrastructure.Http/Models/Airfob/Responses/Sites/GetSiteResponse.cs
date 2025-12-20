namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;

public class GetSiteResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int? ParentId { get; set; }
    public int? DistributorId { get; set; }
    public string MobileId { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string Timezone { get; set; } = default!;
    public string ProviderType { get; set; } = default!;
    public string Properties { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
