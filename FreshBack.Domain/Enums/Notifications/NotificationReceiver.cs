namespace FreshBack.Domain.Enums.Notifications;

[Flags]
public enum NotificationReceiver
{
    Merchant = 1,
    Customer,
    Both = Merchant | Customer
}
