using FluentValidation;
using FreshBack.Application.Dtos.Settings.PaymentMethods;

namespace FreshBack.Application.Validators.Settings.PaymentMethods;

public class PaymentMethodDtoValidator : AbstractValidator<PaymentMethodDto>
{
    public PaymentMethodDtoValidator()
    {
        RuleFor(pm => pm.NameAr)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(pm => pm.NameEn)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);
    }
}
