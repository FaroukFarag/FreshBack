using FluentValidation;
using FreshBack.Application.Dtos.Products;

namespace FreshBack.Application.Validators.Products;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(cp => cp.Code)
            .NotNull()
            .NotEmpty();

        RuleFor(cp => cp.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(cp => cp.NameEn)
            .NotNull()
            .NotEmpty();

        RuleFor(cp => cp.Description)
           .NotNull()
           .NotEmpty();

        RuleFor(cp => cp.DescriptionEn)
            .NotNull()
            .NotEmpty();

        RuleFor(cp => cp.Allergens)
           .NotNull()
           .NotEmpty();

        RuleFor(cp => cp.AllergensEn)
            .NotNull()
            .NotEmpty();

        RuleFor(cp => cp.Warnings)
           .NotNull()
           .NotEmpty();

        RuleFor(cp => cp.WarningsEn)
            .NotNull()
            .NotEmpty();

        RuleFor(cp => cp.Price)
            .NotNull()
            .GreaterThan(0);

        RuleFor(cp => cp.WeightInKg)
            .NotNull()
            .GreaterThan(0);

        RuleFor(cp => cp.MerchantId)
            .NotNull();
    }
}
