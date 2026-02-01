using FluentValidation;
using FreshBack.Application.Dtos.Addresses;

namespace FreshBack.Application.Validators.Addresses;

public class CreateAddressDtoValidator : AbstractValidator<CreateAddressDto>
{
    public CreateAddressDtoValidator()
    {
        RuleFor(ca => ca.Country)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(ca => ca.City)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(ca => ca.Area)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(ca => ca.Street)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(ca => ca.BuildingNo)
            .NotNull();

        RuleFor(ca => ca.Longitude)
            .NotNull();

        RuleFor(ca => ca.Latitude)
            .NotNull();

        RuleFor(ca => ca.MainAddress)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(ca => ca.CustomerId)
            .NotNull();
    }
}
