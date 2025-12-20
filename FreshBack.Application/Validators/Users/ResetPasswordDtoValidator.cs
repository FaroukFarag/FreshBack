using FluentValidation;
using FreshBack.Application.Dtos.Users;

namespace FreshBack.Application.Validators.Users;

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
