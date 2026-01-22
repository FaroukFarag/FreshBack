using FluentValidation;
using FreshBack.Application.Dtos.Branches;

namespace FreshBack.Application.Validators.Branches;

public class ReviewDtoValidator : AbstractValidator<ReviewDto>
{
    public ReviewDtoValidator()
    {
        RuleFor(r => r.Comment)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.Date)
            .NotNull()
            .GreaterThanOrEqualTo(DateTime.Now);

        RuleFor(r => r.UserId)
            .NotNull()
            .GreaterThan(0);

        RuleFor(r => r.MerchantId)
            .NotNull()
            .GreaterThan(0);
    }
}
