using FluentValidation;
using FreshBack.Application.Dtos.Carts;

namespace FreshBack.Application.Validators.Carts;

public class CartDtoValidator : AbstractValidator<CartDto>
{
    public CartDtoValidator()
    {
        RuleFor(c => c.CustomerId)
            .NotNull()
            .GreaterThan(0);
    }
}
