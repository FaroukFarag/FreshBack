using FreshBack.Application.Dtos.Notifications;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Notifications;

namespace FreshBack.Application.Interfaces.Notifications;

public interface INotificationService : IBaseService<
    NotificationDto,
    NotificationDto,
    NotificationDto,
    NotificationDto,
    Notification,
    int>
{
}
