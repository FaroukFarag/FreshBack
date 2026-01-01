using FreshBack.Domain.Interfaces.Repositories.Notifications;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Notifications;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.Notifications;

public class NotificationRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<Notification> specificationCombiner) :
    BaseRepository<Notification, int>(context, specificationCombiner),
    INotificationRepository
{
}
