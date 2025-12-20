using FreshBack.Common.Tokens.Configurations;
using FreshBack.Common.Tokens.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FreshBack.Common.Tokens.Services;

public class TokensService : ITokensService
{
    private readonly IOptionsMonitor<JwtTokenSettings> _settings;

    public TokensService(IOptionsMonitor<JwtTokenSettings> settings)
    {
        _settings = settings;
    }

    public Task<string> GenerateToken(IEnumerable<TokenClaim> tokenClaims)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetTokenClaim(tokenClaims);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return Task.FromResult(token);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_settings.CurrentValue.SecretKey);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private IEnumerable<Claim> GetTokenClaim(IEnumerable<TokenClaim> tokenClaims)
    {
        return tokenClaims.Select(c =>
        {
            return new Claim(c.Name, c.Value);
        }).ToList();
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        return new JwtSecurityToken(
            issuer: _settings.CurrentValue.ValidIssuer,
            audience: _settings.CurrentValue.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.CurrentValue.LifeTime),
            signingCredentials: signingCredentials);
    }
}
