using FreshBack.Application.Dtos.CustomersBranchesFavorite;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.BranchesFavorites;

namespace FreshBack.Application.Interfaces.CustomersBranchesFavorite;

public interface ICustomerBranchFavoriteService : IBaseService<
    CreateCustomerBranchFavoriteDto,
    CustomerBranchFavoriteDto,
    CustomerBranchFavoriteDto,
    CustomerBranchFavoriteDto,
    CustomerBranchFavorite,
    (int CustomerId, int BranchId)>
{
    Task<ResultDto<PagedResult<CustomerBranchFavoriteDto>>> GetCustomerFavoriteBranchesPaginatedAsync(
        PaginatedModelDto paginatedModelDto, int customerId);
}
