using FluentValidation;
using FreshBack.Application.Dtos.OrdersPhotos;

namespace FreshBack.Application.Validators.OrdersPhotos;

public class OrderPhotoDtoValidator : AbstractValidator<CreateOrderPhotoDto>
{
    public OrderPhotoDtoValidator()
    {
        RuleFor(cop => cop.OrderId)
            .NotNull();

        RuleFor(cop => cop.BranchId)
            .NotNull();
    }
}
