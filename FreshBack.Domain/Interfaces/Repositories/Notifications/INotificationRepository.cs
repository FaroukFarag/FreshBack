using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Models.Notifications;

namespace FreshBack.Domain.Interfaces.Repositories.Notifications;

public interface INotificationRepository : IBaseRepository<Notification, int>
{
}
