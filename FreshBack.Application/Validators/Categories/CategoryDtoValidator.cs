using FluentValidation;
using FreshBack.Application.Dtos.Categories;

namespace FreshBack.Application.Validators.Categories;

public class CategoryDtoValidator : AbstractValidator<CategoryDto>
{
    public CategoryDtoValidator()
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
