using AutoMapper;
using FreshBack.Application.Dtos.Settings.Commissions;
using FreshBack.Application.Interfaces.Settings.Commissions;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Settings.Commissions;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Settings.Commissions;

namespace FreshBack.Application.Services.Settings.Commissions;

public class CategoryCommissionService(
    ICategoryCommissionRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : BaseService<
        CreateCategoryCommissionDto,
        CategoryCommissionDto,
        CategoryCommissionDto,
        CategoryCommissionDto,
        CategoryCommission,
        int>(repository, unitOfWork, mapper), ICategoryCommissionService
{
}
