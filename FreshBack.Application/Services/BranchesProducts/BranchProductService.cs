using AutoMapper;
using FreshBack.Application.Dtos.BranchesProducts;
using FreshBack.Application.Interfaces.BranchesProducts;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.BranchesProducts;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.BranchesProducts;

namespace FreshBack.Application.Services.BranchesProducts;

public class BranchProductService(
    IBranchProductRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<
        CreateBranchProductDto,
        BranchProductDto,
        BranchProductDto,
        BranchProductDto,
        BranchProduct,
        (int BranchId, int ProductId)>(repository, unitOfWork, mapper),
    IBranchProductService
{
}
