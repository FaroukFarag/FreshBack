using FreshBack.Application.Dtos.Orders;

namespace FreshBack.Application.Dtos.Notifications;

public class OrderConfirmedNotificationDto : NotificationDto
{
    public OrderConfirmedDto Order { get; set; } = default!;
}
