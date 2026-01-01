namespace FreshBack.Domain.Enums.Notifications;

[Flags]
public enum NotificationReceiver
{
    Merchant,
    Customer,
    Both = Merchant | Customer
}
