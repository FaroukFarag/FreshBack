using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Domain.Enums.Notifications;

namespace FreshBack.Application.Dtos.Notifications;

public class NotificationDto : BaseModelDto<int>
{
    public NotificationReceiver Receiver { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
}
