using FluentValidation;
using FreshBack.Application.Dtos.Settings.Users;

namespace FreshBack.Application.Validators.Settings.Users;

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(rp => rp.UserName)
            .NotEmpty();

        RuleFor(rp => rp.OldPassword)
            .NotEmpty();

        RuleFor(rp => rp.NewPassword)
            .NotEmpty();
    }
}
