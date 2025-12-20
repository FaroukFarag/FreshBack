using AccessControlSystem.Infrastructure.Http.Configurations;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Auth;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Auth;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AccessControlSystem.Infrastructure.Http.Handlers.Airfob;

public class AirfobAuthHandler(HttpClient client, IOptions<AirfobSettings> settings) : DelegatingHandler
{
    private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));
    private readonly AirfobSettings _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    private string _accessToken = string.Empty;
    private DateTime _tokenExpiry = DateTime.MinValue;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await GetAccessTokenAsync(cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiry)
        {
            return _accessToken;
        }

        await RefreshTokenAsync(cancellationToken);

        return _accessToken;
    }

    private async Task RefreshTokenAsync(CancellationToken cancellationToken)
    {
        var response = await _client.PostAsJsonAsync(
            "v1/login",
            new AirfobLoginRequest
            {
                Username = _settings.Username,
                Password = _settings.Password
            }, cancellationToken);

        var authResponse = await response.Content.ReadFromJsonAsync<AirfobLoginResponse>(cancellationToken: cancellationToken);

        _accessToken = authResponse!.AccessToken;
        _tokenExpiry = GetTokenExpiry(_accessToken);
    }

    private static DateTime GetTokenExpiry(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

            if (long.TryParse(expClaim, out var unixTime))
            {
                return DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
            }

            return DateTime.UtcNow.AddSeconds(3600 - 60);
        }

        catch
        {
            return DateTime.UtcNow.AddMinutes(5);
        }
    }
}
