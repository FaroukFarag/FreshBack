using FluentValidation;
using FreshBack.Application.Dtos.Users;

namespace FreshBack.Application.Validators.Users;

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
