using FluentValidation;
using FreshBack.Application.Dtos.CustomersBranchesFavorite;

namespace FreshBack.Application.Validators.CustomersBranchesFavorite;

public class CreateCustomerBranchFavoriteDtoValidator :
    AbstractValidator<CreateCustomerBranchFavoriteDto>
{
    public CreateCustomerBranchFavoriteDtoValidator()
    {
        RuleFor(ccbf => ccbf.CustomerId)
            .NotNull();

        RuleFor(ccbf => ccbf.BranchId)
            .NotNull();
    }
}
