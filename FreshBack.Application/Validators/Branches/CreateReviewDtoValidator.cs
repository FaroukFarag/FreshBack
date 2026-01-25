using FluentValidation;
using FreshBack.Application.Dtos.Branches;

namespace FreshBack.Application.Validators.Branches;

public class CreateReviewDtoValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewDtoValidator()
    {
        RuleFor(r => r.Rating)
            .NotNull()
            .InclusiveBetween(1, 5);

        RuleFor(r => r.Comment)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.Date)
            .NotNull()
            .GreaterThanOrEqualTo(DateTime.Now);

        RuleFor(r => r.CustomerId)
            .NotNull()
            .GreaterThan(0);

        RuleFor(r => r.MerchantId)
            .NotNull()
            .GreaterThan(0);
    }
}
