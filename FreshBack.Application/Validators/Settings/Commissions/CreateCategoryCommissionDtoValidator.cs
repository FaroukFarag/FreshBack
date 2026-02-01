using FluentValidation;
using FreshBack.Application.Dtos.Settings.Commissions;

namespace FreshBack.Application.Validators.Settings.Commissions;

public class CreateCategoryCommissionDtoValidator :
    AbstractValidator<CreateCategoryCommissionDto>
{
    public CreateCategoryCommissionDtoValidator()
    {
        RuleFor(ccc => ccc.CommissionId)
            .NotNull();

        RuleFor(ccc => ccc.CategoryId)
            .NotNull();

        RuleFor(ccc => ccc.PercentageOfTotal)
            .NotNull()
            .InclusiveBetween(0, 100);
    }
}
