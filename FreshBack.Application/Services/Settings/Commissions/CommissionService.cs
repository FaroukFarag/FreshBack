using AutoMapper;
using FreshBack.Application.Dtos.Settings.Commissions;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Settings.Commissions;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Enums.Settings.Commissions;
using FreshBack.Domain.Interfaces.Repositories.Settings.Commissions;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Settings.Commissions;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.Settings.Commissions;

public class CommissionService(
    ICommissionRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICategoryCommissionRepository categoryCommissionRepository) : BaseService<
        CreateCommissionDto,
        CommissionDto,
        CommissionDto,
        CreateCommissionDto,
        Commission,
        int>(repository, unitOfWork, mapper), ICommissionService
{
    private readonly ICommissionRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ICategoryCommissionRepository _categoryCommissionRepository =
        categoryCommissionRepository;

    public async override Task<ResultDto<IEnumerable<CommissionDto>>> GetAllAsync()
    {
        return await ExecuteServiceCallAsync(
            "Get All Commissions",
            async () =>
            {
                var commissions = await _repository
                    .GetAllAsync(new BaseSpecification<Commission>
                    {
                        Includes =
                        [
                            c => c.CategoryCommissions
                        ]
                    });

                return _mapper.Map<IEnumerable<CommissionDto>>(commissions);
            });
    }

    public async Task<ResultDto<decimal>> CalculateCommissionAsync(
        int categoryId, decimal orderTotal)
    {
        var commissionsResult = await GetAllAsync();

        if (!commissionsResult.Succeeded)
            return ResultDto<decimal>.CreateFailResult("Failed to calculate commission");

        var commission = commissionsResult.ResultData.FirstOrDefault();

        if (commission is null)
            ResultDto<decimal>.CreateSuccessResult(0);

        var value = commission!.Type switch
        {
            CommissionType.FixedAmount => commission.FixedAmount ?? 0,
            CommissionType.MerchantCategory => CalculateCategoryCommission(
                commission, categoryId, orderTotal),
            _ => 0
        };

        return ResultDto<decimal>.CreateSuccessResult(value);
    }

    private static decimal CalculateCategoryCommission(
        CommissionDto commissionDto, int categoryId, decimal orderTotal)
    {
        var categoryCommission = commissionDto.CategoryCommissions
            .FirstOrDefault(c => c.CategoryId == categoryId);

        return categoryCommission != null
            ? orderTotal * (categoryCommission.PercentageOfTotal / 100)
            : 0;
    }
}
