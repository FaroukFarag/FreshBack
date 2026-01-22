using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Branches;

namespace FreshBack.Application.Interfaces.Branches;

public interface IBranchService : IBaseService<
    CreateBranchDto,
    BranchDto,
    BranchDto,
    BranchDto,
    Branch,
    int>
{
    Task<ResultDto<PagedResult<BranchDto>>> GetAllPaginatedAsync(
        BranchPaginatedModelDto paginatedModelDto);
    Task<ResultDto<PagedResult<BranchDto>>> GetOtherBranchesPaginatedAsync(
        OtherBranchesPaginatedModelDto paginatedModelDto);
}
