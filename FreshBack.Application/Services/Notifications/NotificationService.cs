using AutoMapper;
using FreshBack.Application.Dtos.Notifications;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Notifications;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Notifications;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Notifications;

namespace FreshBack.Application.Services.Notifications;

public class NotificationService(
    INotificationRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    INotificationSender sender) : BaseService<
    NotificationDto,
    NotificationDto,
    NotificationDto,
    NotificationDto,
    Notification,
    int>(repository, unitOfWork, mapper), INotificationService
{
    private readonly INotificationRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly INotificationSender _sender = sender;

    public async override Task<ResultDto<NotificationDto>> CreateAsync(NotificationDto notificationDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Notification",
            action: async () =>
            {
                var notification = await _repository.CreateAsync(
                    _mapper.Map<Notification>(notificationDto));
                var savedSuccessfully = await _unitOfWork.Complete();

                if (!savedSuccessfully)
                    throw new Exception($"Failed to create notification");

                await _sender.SendAsync(notificationDto);

                return _mapper.Map<NotificationDto>(notification);
            });
    }
}
