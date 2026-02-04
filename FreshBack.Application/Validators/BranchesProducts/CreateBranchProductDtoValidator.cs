using FluentValidation;
using FreshBack.Application.Dtos.BranchesProducts;

namespace FreshBack.Application.Validators.BranchesProducts;

public class CreateBranchProductDtoValidator : AbstractValidator<CreateBranchProductDto>
{
    public CreateBranchProductDtoValidator()
    {
        RuleFor(cbp => cbp.BranchId)
            .NotNull();

        RuleFor(cbp => cbp.ProductId)
            .NotNull();

        RuleFor(cbp => cbp.Discount)
            .NotNull();

        RuleFor(cbp => cbp.ExpiryDate)
            .NotNull();

        RuleFor(cbp => cbp.Quantity)
            .NotNull()
            .GreaterThan(0);

        RuleFor(cbp => cbp.StartDeliveryDate)
            .NotNull();

        RuleFor(cbp => cbp.EndDeliveryDate)
            .NotNull();

        RuleFor(cbp => cbp.Status)
            .NotNull();
    }
}
