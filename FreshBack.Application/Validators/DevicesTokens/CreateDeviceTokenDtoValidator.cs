using FluentValidation;
using FreshBack.Application.Dtos.DevicesTokens;

namespace FreshBack.Application.Validators.DevicesTokens;

public class CreateDeviceTokenDtoValidator : AbstractValidator<CreateDeviceTokenDto>
{
    public CreateDeviceTokenDtoValidator()
    {
        RuleFor(cdt => cdt.Token)
            .NotEmpty()
            .NotNull();
    }
}
