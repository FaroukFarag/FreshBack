using FreshBack.Application.Dtos.Notifications;
using FreshBack.Application.Interfaces.Notifications;
using FreshBack.Domain.Enums.Notifications;
using FreshBack.Domain.Enums.Roles;
using Microsoft.AspNetCore.SignalR;

namespace FreshBack.Application.SignalR.Notifications;

public class SignalRNotificationSender : INotificationSender
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationSender(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendAsync(NotificationDto notification)
    {
        var tasks = new List<Task>();

        if (notification.Receiver.HasFlag(NotificationReceiver.Merchant))
        {
            tasks.Add(
                _hubContext.Clients
                    .Group(nameof(RoleNames.Merchant))
                    .SendAsync("ReceiveNotification", notification)
            );
        }

        if (notification.Receiver.HasFlag(NotificationReceiver.Customer))
        {
            tasks.Add(
                _hubContext.Clients
                    .Group(nameof(RoleNames.Customer))
                    .SendAsync("ReceiveNotification", notification)
            );
        }

        await Task.WhenAll(tasks);
    }
}
