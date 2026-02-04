using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Models.BranchesProducts;

namespace FreshBack.Domain.Interfaces.Repositories.BranchesProducts;

public interface IBranchProductRepository : IBaseRepository<
    BranchProduct, (int BranchId, int ProductId)>
{
}
