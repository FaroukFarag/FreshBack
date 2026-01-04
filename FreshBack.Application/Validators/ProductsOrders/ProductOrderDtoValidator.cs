using FluentValidation;
using FreshBack.Application.Dtos.ProductsOrders;

namespace FreshBack.Application.Validators.ProductsOrders;

public class ProductOrderDtoValidator : AbstractValidator<ProductOrderDto>
{
    public ProductOrderDtoValidator()
    {
        RuleFor(po => po.ProductId)
            .NotNull();

        RuleFor(po => po.OrderId)
            .NotNull();

        RuleFor(po => po.Quantity)
            .NotNull()
            .GreaterThan(0);
    }
}
