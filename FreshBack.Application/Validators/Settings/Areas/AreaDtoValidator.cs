using FluentValidation;
using FreshBack.Application.Dtos.Settings.Areas;

namespace FreshBack.Application.Validators.Settings.Areas;

public class AreaDtoValidator : AbstractValidator<AreaDto>
{
    public AreaDtoValidator()
    {
        RuleFor(a => a.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(a => a.DeliveryFees)
            .NotNull()
            .GreaterThan(0);
    }
}
