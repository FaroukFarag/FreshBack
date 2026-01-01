using Microsoft.AspNetCore.SignalR;

namespace FreshBack.Application.SignalR.Notifications;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var role = Context.User?.FindFirst("Role")?.Value;

        if (!string.IsNullOrWhiteSpace(role))
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                role
            );
        }

        await base.OnConnectedAsync();
    }
}
