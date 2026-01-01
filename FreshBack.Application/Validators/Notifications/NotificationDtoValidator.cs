using FluentValidation;
using FreshBack.Application.Dtos.Notifications;

namespace FreshBack.Application.Validators.Notifications;

public class NotificationDtoValidator : AbstractValidator<NotificationDto>
{
    public NotificationDtoValidator()
    {
        RuleFor(n => n.Receiver)
            .NotNull();

        RuleFor(n => n.Title)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(n => n.Content)
            .NotNull()
            .NotEmpty();
    }
}
