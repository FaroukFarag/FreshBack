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

        RuleFor(p => p.Discount)
            .NotNull();

        RuleFor(p => p.CreationDate)
            .NotNull();

        RuleFor(p => p.ExpiryDate)
            .NotNull();

        RuleFor(p => p.Status)
            .NotNull();

        RuleFor(p => p.MerchantId)
            .NotNull();
    }
}
