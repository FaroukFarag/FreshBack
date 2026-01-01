using FreshBack.Application.Dtos.Notifications;

namespace FreshBack.Application.Interfaces.Notifications;

public interface INotificationSender
{
    Task SendAsync(NotificationDto notification);
}
