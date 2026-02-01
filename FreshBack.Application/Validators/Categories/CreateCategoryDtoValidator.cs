using FluentValidation;
using FreshBack.Application.Dtos.Categories;

namespace FreshBack.Application.Validators.Categories;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(c => c.NameEn)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);
    }
}
