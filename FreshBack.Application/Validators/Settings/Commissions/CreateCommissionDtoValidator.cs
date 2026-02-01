using FluentValidation;
using FreshBack.Application.Dtos.Settings.Commissions;

namespace FreshBack.Application.Validators.Settings.Commissions;

public class CreateCommissionDtoValidator : AbstractValidator<CreateCommissionDto>
{
    public CreateCommissionDtoValidator()
    {
        RuleFor(cc => cc.Type)
            .NotNull();

        RuleFor(cc => cc.FixedAmount)
            .GreaterThan(0);
    }
}
