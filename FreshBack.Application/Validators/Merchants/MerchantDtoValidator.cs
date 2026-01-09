using FluentValidation;
using FreshBack.Application.Dtos.Merchants;

namespace FreshBack.Application.Validators.Merchants;

public class MerchantDtoValidator : AbstractValidator<MerchantDto>
{
    public MerchantDtoValidator()
    {
        RuleFor(m => m.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(m => m.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .MaximumLength(25);

        RuleFor(m => m.AreaId)
            .NotNull();
    }
}
