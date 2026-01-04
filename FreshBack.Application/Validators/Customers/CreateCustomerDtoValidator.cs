using FluentValidation;
using FreshBack.Application.Dtos.Customers;

namespace FreshBack.Application.Validators.Customers;

public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);

        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .Matches(@"^(?:\+20|0)?1[0125]\d{8}$")
            .WithMessage("Invalid mobile number")
            .MaximumLength(25);

        RuleFor(c => c.Country)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.City)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.Area)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.Street)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(c => c.BuildingNo)
            .NotNull();
    }
}
