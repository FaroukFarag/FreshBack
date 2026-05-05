namespace FreshBack.Application.Configurations;

public class FirebaseSettings
{
    public const string SectionName = "FirebaseSettings";

    public string ProjectId { get; set; } = default!;
    public string CredentialsPath { get; set; } = default!;
}
