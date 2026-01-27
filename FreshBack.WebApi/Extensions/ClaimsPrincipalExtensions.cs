using System.Security.Claims;

namespace FreshBack.WebApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        if (int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier)!, out int userId))
            return userId;

        return 0;
    }
}
