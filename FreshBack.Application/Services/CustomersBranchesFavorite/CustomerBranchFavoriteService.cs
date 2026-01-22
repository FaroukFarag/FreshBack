using AutoMapper;
using FreshBack.Application.Dtos.CustomersBranchesFavorite;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.CustomersBranchesFavorite;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Common.Extensions;
using FreshBack.Domain.Interfaces.Repositories.CustomersBranchesFavorite;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.BranchesFavorites;
using FreshBack.Domain.Models.Shared;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.CustomersBranchesFavorite;

public class CustomerBranchFavoriteService(
    ICustomerBranchFavoriteRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : BaseService<
    CreateCustomerBranchFavoriteDto,
    CustomerBranchFavoriteDto,
    CustomerBranchFavoriteDto,
    CustomerBranchFavoriteDto,
    CustomerBranchFavorite,
    (int CustomerId, int BranchId)>(repository, unitOfWork, mapper),
    ICustomerBranchFavoriteService
{
    private readonly ICustomerBranchFavoriteRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResultDto<PagedResult<CustomerBranchFavoriteDto>>>
        GetCustomerFavoriteBranchesPaginatedAsync(
            PaginatedModelDto paginatedModelDto,
            int customerId)
    {
        return await ExecuteServiceCallAsync(
            "Get Customer Favorite Branches Paginated",
            async () =>
            {
                var spec = new BaseSpecification<CustomerBranchFavorite>
                {
                    Criteria = cbf => cbf.CustomerId == customerId,
                    IncludeChains =
                    [
                        new IncludeChain<CustomerBranchFavorite>
                        {
                            InitialInclude = cbf => cbf.Branch,
                            ThenIncludes = [
                                b => (b as Branch)!.Merchant
                            ]
                        }
                    ]
                };
                var paginatedModel = _mapper.Map<PaginatedModel>(paginatedModelDto);
                var (branches, totalCount) =
                    await _repository.GetAllPaginatedAsync(
                        paginatedModel,
                        spec);

                return new PagedResult<CustomerBranchFavoriteDto>(
                    _mapper.Map<IReadOnlyList<CustomerBranchFavoriteDto>>(branches),
                    totalCount);
            });
    }
}
