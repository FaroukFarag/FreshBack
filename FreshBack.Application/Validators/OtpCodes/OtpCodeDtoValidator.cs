using FluentValidation;
using FreshBack.Application.Dtos.OtpCodes;

namespace FreshBack.Application.Validators.OtpCodes;

public class OtpCodeDtoValidator : AbstractValidator<OtpCodeDto>
{
    public OtpCodeDtoValidator()
    {
        RuleFor(otp => otp.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .Matches(@"^(?:\+20|0)?1[0125]\d{8}$")
            .WithMessage("Invalid mobile number")
            .MaximumLength(25);

        RuleFor(otp => otp.Otp)
            .NotNull()
            .NotEmpty()
            .MaximumLength(10);
    }
}
