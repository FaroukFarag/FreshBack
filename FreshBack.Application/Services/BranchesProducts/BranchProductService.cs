using AutoMapper;
using FreshBack.Application.Dtos.BranchesProducts;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.BranchesProducts;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.BranchesProducts;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.BranchesProducts;
using FreshBack.Domain.Models.Shared;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.BranchesProducts;

public class BranchProductService(
    IBranchProductRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<
        CreateBranchProductDto,
        BranchProductDto,
        BranchProductDto,
        CreateBranchProductDto,
        BranchProduct,
        (int BranchId, int ProductId)>(repository, unitOfWork, mapper),
    IBranchProductService
{
    private readonly IBranchProductRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ResultDto<PagedResult<BranchProductDto>>>
        GetBranchesRemainingProductsPaginatedAsync(PaginatedModelDto paginatedModelDto)
    {
        return await ExecuteServiceCallAsync(
            "Get Branches Remaining Products Paginated",
            async () =>
            {
                var spec = new BaseSpecification<BranchProduct>
                {
                    Criteria = bp => (bp.Quantity == 0 || bp.ExpiryDate < DateTime.Now),
                    Includes =
                    [
                        pb => pb.Product
                    ]
                };

                var paginatedModel = _mapper.Map<PaginatedModel>(paginatedModelDto);

                var (branchProducts, totalCount) =
                    await _repository.GetAllPaginatedAsync(
                        paginatedModel,
                        spec);

                return new PagedResult<BranchProductDto>(
                    _mapper.Map<IReadOnlyList<BranchProductDto>>(branchProducts),
                    totalCount);
            });
    }
}
