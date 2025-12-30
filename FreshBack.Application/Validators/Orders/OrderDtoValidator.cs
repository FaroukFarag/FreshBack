using FluentValidation;
using FreshBack.Application.Dtos.Orders;

namespace FreshBack.Application.Validators.Orders;

public class OrderDtoValidator : AbstractValidator<OrderDto>
{
    public OrderDtoValidator()
    {
        RuleFor(o => o.Number)
            .Null();

        RuleFor(o => o.CustomerName)
            .Null()
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(o => o.CustomerEmail)
            .Null()
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(o => o.CustomerMobileNumber)
            .Null()
            .NotEmpty()
            .MaximumLength(25);

        RuleFor(o => o.CreationDate)
            .Null();

        RuleFor(o => o.Status)
            .Null();

        RuleFor(o => o.PaymentMethod)
            .Null();

        RuleFor(o => o.Price)
            .Null();

        RuleFor(o => o.Discount)
            .Null();

        RuleFor(o => o.Fees)
            .Null();

        RuleFor(o => o.MerchantId)
            .Null();
    }
}
