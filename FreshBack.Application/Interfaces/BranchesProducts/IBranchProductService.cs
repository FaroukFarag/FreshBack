using FreshBack.Application.Dtos.BranchesProducts;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.BranchesProducts;

namespace FreshBack.Application.Interfaces.BranchesProducts;

public interface IBranchProductService : IBaseService<
    CreateBranchProductDto,
    BranchProductDto,
    BranchProductDto,
    CreateBranchProductDto,
    BranchProduct,
    (int BranchId, int ProductId)>
{
    Task<ResultDto<PagedResult<BranchProductDto>>>
        GetBranchesRemainingProductsPaginatedAsync(PaginatedModelDto paginatedModelDto);
}
