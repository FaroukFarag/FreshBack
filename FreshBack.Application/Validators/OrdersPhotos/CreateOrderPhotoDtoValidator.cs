using FluentValidation;
using FreshBack.Application.Dtos.OrdersPhotos;

namespace FreshBack.Application.Validators.OrdersPhotos;

public class CreateOrderPhotoDtoValidator : AbstractValidator<CreateOrderPhotoDto>
{
    public CreateOrderPhotoDtoValidator()
    {
        RuleFor(cop => cop.OrderId)
            .NotNull();

        RuleFor(cop => cop.BranchId)
            .NotNull();
    }
}
