using FluentValidation;
using FreshBack.Application.Dtos.Addresses;

namespace FreshBack.Application.Validators.Addresses;

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(a => a.Country)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(a => a.City)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(a => a.Area)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(a => a.Street)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(a => a.BuildingNo)
            .NotNull();
    }
}
