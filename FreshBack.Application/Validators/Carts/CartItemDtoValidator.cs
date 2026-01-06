using FluentValidation;
using FreshBack.Application.Dtos.Carts;

namespace FreshBack.Application.Validators.Carts;

public class CartItemDtoValidator : AbstractValidator<CartItemDto>
{
    public CartItemDtoValidator()
    {
        RuleFor(ci => ci.CartId)
            .NotNull()
            .GreaterThan(0);

        RuleFor(ci => ci.ProductId)
            .NotNull()
            .GreaterThan(0);

        RuleFor(ci => ci.Quantity)
            .NotNull()
            .GreaterThan(0);

        RuleFor(ci => ci.Price)
            .NotNull()
            .GreaterThan(0);
    }
}
