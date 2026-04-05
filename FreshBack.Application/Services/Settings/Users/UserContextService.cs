using FreshBack.Common.Interfaces.Settings.Users;
using Microsoft.AspNetCore.Http;

namespace FreshBack.Application.Services.Settings.Users;

public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public bool IsAuthenticated()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        return user?.Identity?.IsAuthenticated ?? false;
    }

    public bool HasMerchantId()
    {
        return _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == "merchantId")?.Value is not null;
    }

    public bool IsAdmin()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        return user?.IsInRole("Admin") ?? false;
    }

    public int GetUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == "userId")?.Value;

        if (int.TryParse(userIdClaim, out int userId))
        {
            return userId;
        }

        return default!;
    }

    public int GetMerchantId()
    {
        var merchantIdClaim = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == "merchantId")?.Value;

        if (int.TryParse(merchantIdClaim, out int merchantId))
        {
            return merchantId;
        }

        return default!;
    }
}
