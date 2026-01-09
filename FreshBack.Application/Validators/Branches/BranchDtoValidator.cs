using FluentValidation;
using FreshBack.Application.Dtos.Branches;

namespace FreshBack.Application.Validators.Branches;

public class BranchDtoValidator : AbstractValidator<BranchDto>
{
    public BranchDtoValidator()
    {
        RuleFor(b => b.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(b => b.NameEn)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(b => b.Neighborhood)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(b => b.NeighborhoodEn)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(b => b.Latitude)
            .NotNull();

        RuleFor(b => b.Longitude)
            .NotNull();

        RuleFor(b => b.OpeningTime)
            .NotNull();

        RuleFor(b => b.ClosingTime)
            .NotNull()
            .GreaterThan(b => b.OpeningTime);

        RuleFor(b => b.Status)
           .NotNull();

        RuleFor(b => b.AreaId)
           .NotNull();

        RuleFor(b => b.MerchantId)
            .NotNull();
    }
}
