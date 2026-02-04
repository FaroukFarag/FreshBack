using FreshBack.Application.Dtos.BranchesProducts;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.BranchesProducts;

namespace FreshBack.Application.Interfaces.BranchesProducts;

public interface IBranchProductService : IBaseService<
    CreateBranchProductDto,
    BranchProductDto,
    BranchProductDto,
    BranchProductDto,
    BranchProduct,
    (int BranchId, int ProductId)>
{
}
