using FluentValidation;
using FreshBack.Application.Dtos.Products;

namespace FreshBack.Application.Validators.Products;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(p => p.Code)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.NameEn)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.Description)
           .NotNull()
           .NotEmpty();

        RuleFor(p => p.DescriptionEn)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.Allergens)
           .NotNull()
           .NotEmpty();

        RuleFor(p => p.AllergensEn)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.Warnings)
           .NotNull()
           .NotEmpty();

        RuleFor(p => p.WarningsEn)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.Price)
            .NotNull()
            .GreaterThan(0);

        RuleFor(p => p.WeightInKg)
            .NotNull()
            .GreaterThan(0);

        RuleFor(p => p.MerchantId)
            .NotNull();
    }
}
