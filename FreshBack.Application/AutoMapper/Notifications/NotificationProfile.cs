using AutoMapper;
using FreshBack.Application.Dtos.Notifications;
using FreshBack.Domain.Models.Notifications;

namespace FreshBack.Application.AutoMapper.Notifications;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationDto>().ReverseMap();
    }
}
