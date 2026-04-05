using FluentValidation;
using FreshBack.Application.Dtos.Branches;

namespace FreshBack.Application.Validators.Branches;

public class CreateReviewImageDtoValidator : AbstractValidator<CreateReviewImageDto>
{
    public CreateReviewImageDtoValidator()
    {
        RuleFor(cri => cri.ReviewId)
            .NotNull();
    }
}
