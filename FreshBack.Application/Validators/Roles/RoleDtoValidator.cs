using FluentValidation;
using FreshBack.Application.Dtos.Roles;

namespace FreshBack.Application.Validators.Roles;

public class RoleDtoValidator : AbstractValidator<RoleDto>
{
    public RoleDtoValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
