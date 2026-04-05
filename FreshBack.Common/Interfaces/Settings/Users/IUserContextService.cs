namespace FreshBack.Common.Interfaces.Settings.Users;

public interface IUserContextService
{
    bool IsAuthenticated();
    bool HasMerchantId();
    bool IsAdmin();
    int GetUserId();
    int GetMerchantId();
}
