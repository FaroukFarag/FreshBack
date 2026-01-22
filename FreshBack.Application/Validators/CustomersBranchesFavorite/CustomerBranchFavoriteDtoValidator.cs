using FluentValidation;
using FreshBack.Application.Dtos.CustomersBranchesFavorite;

namespace FreshBack.Application.Validators.CustomersBranchesFavorite;

public class CustomerBranchFavoriteDtoValidator :
    AbstractValidator<CustomerBranchFavoriteDto>
{
    public CustomerBranchFavoriteDtoValidator()
    {
        RuleFor(cbf => cbf.CustomerId)
            .NotNull();

        RuleFor(cbf => cbf.BranchId)
            .NotNull();
    }
}
