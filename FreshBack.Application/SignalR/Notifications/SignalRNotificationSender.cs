using FreshBack.Application.Dtos.Notifications;
using FreshBack.Application.Interfaces.Notifications;
using FreshBack.Domain.Enums.Roles;
using Microsoft.AspNetCore.SignalR;

namespace FreshBack.Application.SignalR.Notifications;

public class SignalRNotificationSender(IHubContext<NotificationHub> hubContext) : INotificationSender
{
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;

    public async Task SendAsync(NotificationDto notification)
    {
        await _hubContext.Clients
            .Group(nameof(RoleNames.Merchant))
            .SendAsync("ReceiveNotification", notification);
    }
}
