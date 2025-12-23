using FluentValidation;
using FreshBack.Application.Dtos.Settings.Users;

namespace FreshBack.Application.Validators.Settings.Users;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(l => l.UserName)
            .NotEmpty();

        RuleFor(l => l.Password)
            .NotEmpty();
    }
}
