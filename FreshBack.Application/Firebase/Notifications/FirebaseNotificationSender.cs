using FirebaseAdmin.Messaging;
using FreshBack.Application.Dtos.Notifications;
using FreshBack.Application.Interfaces.Notifications;
using FreshBack.Domain.Interfaces.Repositories.DevicesTokens;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.DevicesTokens;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Firebase.Notifications;

public class FirebaseNotificationSender(
    IDeviceTokenRepository deviceTokenRepository,
    IUnitOfWork unitOfWork) : INotificationSender
{
    private readonly IDeviceTokenRepository _deviceTokenRepository = deviceTokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task SendAsync(NotificationDto notification)
    {
        var tokens = await _deviceTokenRepository.GetAllAsync();

        await SendToTokensAsync([.. tokens], notification);
    }

    public async Task SendToCustomerAsync(int customerId, OrderConfirmedNotificationDto notification)
    {
        var tokens = await _deviceTokenRepository.GetAllAsync(new BaseSpecification<DeviceToken>
        {
            Criteria = dt => dt.CustomerId == customerId && dt.IsActive
        });

        await SendToTokensAsync([.. tokens], notification);
    }

    private async Task SendToTokensAsync(
        List<DeviceToken> deviceTokens,
        NotificationDto notification)
    {
        if (deviceTokens.Count == 0)
            return;

        var message = new MulticastMessage
        {
            Tokens = [.. deviceTokens.Select(dt => dt.Token)],
            Notification = new Notification
            {
                Title = notification.Title,
                Body = notification.Content
            },
            Android = new AndroidConfig
            {
                Priority = Priority.High,
                Notification = new AndroidNotification
                {
                    ChannelId = "default_channel",
                    Sound = "default"
                }
            },

            Data = new Dictionary<string, string>
            {
                { "title", notification.Title },
                { "content", notification.Content }
            }
        };

        var response = await FirebaseMessaging
            .DefaultInstance
            .SendEachForMulticastAsync(message);

        await RemoveInvalidTokens(deviceTokens, response);
    }

    private async Task SendToTokensAsync(
        List<DeviceToken> deviceTokens,
        OrderConfirmedNotificationDto notification)
    {
        if (deviceTokens.Count == 0)
            return;

        var message = new MulticastMessage
        {
            Tokens = [.. deviceTokens.Select(dt => dt.Token)],
            Notification = new Notification
            {
                Title = notification.Title,
                Body = notification.Content
            },
            Android = new AndroidConfig
            {
                Priority = Priority.High,
                Notification = new AndroidNotification
                {
                    ChannelId = "default_channel",
                    Sound = "default"
                }
            },

            Data = new Dictionary<string, string>
            {
                { "OrderId", notification.Order.Id.ToString() },
                { "BranchId", notification.Order.BranchId.ToString() },
                { "OrderId", notification.Order.BranchName },
                { "MerchantId", notification.Order.MerchantId.ToString() },
                { "OrderId", notification.Order.MerchantName }
            }
        };

        var response = await FirebaseMessaging
            .DefaultInstance
            .SendEachForMulticastAsync(message);

        await RemoveInvalidTokens(deviceTokens, response);
    }

    private async Task RemoveInvalidTokens(
        List<DeviceToken> deviceTokens,
        BatchResponse response)
    {
        var failedTokens = new List<DeviceToken>();

        for (int i = 0; i < response.Responses.Count; i++)
        {
            if (!response.Responses[i].IsSuccess)
            {
                failedTokens.Add(deviceTokens[i]);
            }
        }

        if (failedTokens.Count > 0)
        {
            _deviceTokenRepository.DeleteRange(failedTokens);

            await _unitOfWork.Complete();
        }
    }
}
