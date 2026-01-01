using FreshBack.Domain.Enums.Notifications;
using FreshBack.Domain.Models.Abstraction;

namespace FreshBack.Domain.Models.Notifications;

public class Notification : BaseModel<int>
{
    public NotificationReceiver Receiver { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
}
