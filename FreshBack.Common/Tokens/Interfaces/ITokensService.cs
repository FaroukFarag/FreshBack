namespace FreshBack.Common.Tokens.Interfaces;

public interface ITokensService
{
    Task<string> GenerateToken(IEnumerable<TokenClaim> claims);
}
