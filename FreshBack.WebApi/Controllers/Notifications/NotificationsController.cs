using FreshBack.Application.Dtos.Notifications;
using FreshBack.Application.Interfaces.Notifications;
using FreshBack.Domain.Models.Notifications;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Notifications;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController(INotificationService service) :
    BaseController<INotificationService, NotificationDto, NotificationDto,
        NotificationDto, NotificationDto, Notification, int>(service)
{
}
