using FirebaseAdmin;
using FreshBack.Application.Configurations;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;

namespace FreshBack.WebApi.Firebase;

public static class FirebaseInitializer
{
    public static void Initialize(IServiceProvider services)
    {
        var settings = services
            .GetRequiredService<IOptions<FirebaseSettings>>()
            .Value;

        var env = services
            .GetRequiredService<IWebHostEnvironment>();

        var fullPath = Path.Combine(
            env.ContentRootPath,
            settings.CredentialsPath
        );

        if (FirebaseApp.DefaultInstance == null)
        {
            var credential =
                CredentialFactory
                    .FromFile<ServiceAccountCredential>(fullPath)
                    .ToGoogleCredential();

            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential
            });
        }
    }
}
