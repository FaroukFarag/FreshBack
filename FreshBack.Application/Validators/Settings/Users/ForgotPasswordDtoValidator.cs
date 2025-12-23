using FluentValidation;
using FreshBack.Application.Dtos.Settings.Users;

namespace FreshBack.Application.Validators.Settings.Users;

public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordDtoValidator()
    {
        RuleFor(fp => fp.UserName)
            .NotEmpty();

        RuleFor(fp => fp.NewPassword)
            .NotEmpty();
    }
}
