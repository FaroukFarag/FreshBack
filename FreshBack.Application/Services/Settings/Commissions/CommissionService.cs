using AutoMapper;
using FreshBack.Application.Dtos.Categories;
using FreshBack.Application.Dtos.Settings.Commissions;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Settings.Commissions;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Enums.Settings.Commissions;
using FreshBack.Domain.Interfaces.Repositories.Categories;
using FreshBack.Domain.Interfaces.Repositories.Orders;
using FreshBack.Domain.Interfaces.Repositories.Settings.Commissions;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Orders;
using FreshBack.Domain.Models.Settings.Commissions;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.Settings.Commissions;

public class CommissionService(
    ICommissionRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICategoryCommissionRepository categoryCommissionRepository,
    ICategoryRepository categoryRepository,
    IOrderRepository orderRepository) : BaseService<
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
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;

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
        DateTime? from = null,
        DateTime? to = null)
    {
        var commissionsResult = await GetAllAsync();

        if (!commissionsResult.Succeeded)
            return ResultDto<decimal>.CreateFailResult("Failed to calculate commission");

        var commission = commissionsResult.ResultData.FirstOrDefault();

        if (commission is null)
            return ResultDto<decimal>.CreateSuccessResult(0);

        var value = commission.Type switch
        {
            CommissionType.FixedAmount => await CalculateFixedAmountCommission(
                commission.FixedAmount, from, to),
            CommissionType.MerchantCategory => await CalculateCategoryCommission(
                commission, from, to),
            _ => 0
        };

        return ResultDto<decimal>.CreateSuccessResult(value);
    }

    private async Task<decimal> CalculateFixedAmountCommission(
        decimal? fixedAmount,
        DateTime? from = null,
        DateTime? to = null)
    {
        if (fixedAmount is null)
            return 0;

        var totalOrders = await _orderRepository.GetCountAsync(
            new BaseSpecification<Order>
            {
                Criteria = o =>
                    (!from.HasValue || o.CreationDate >= from.Value) &&
                    (!to.HasValue || o.CreationDate < to.Value)
            });

        return totalOrders * fixedAmount.Value;
    }

    private async Task<decimal> CalculateCategoryCommission(
        CommissionDto commissionDto,
        DateTime? from = null,
        DateTime? to = null)
    {
        var categoriesRevenues = await _categoryRepository
            .GetAllAsync(c => new TotalCategoryRevenuesDto
            {
                CategoryId = c.Id,
                Amount = c.Branches.SelectMany(b => b.Orders)
                            .Where(o =>
                                (!from.HasValue || o.CreationDate >= from.Value) &&
                                (!to.HasValue || o.CreationDate < to.Value))
                            .SelectMany(o => o.ProductsOrders)
                            .Sum(po => po.Quantity * po.Product.Price) -
                        c.Branches.SelectMany(b => b.Orders)
                            .Where(o =>
                                (!from.HasValue || o.CreationDate >= from.Value) &&
                                (!to.HasValue || o.CreationDate < to.Value))
                            .Sum(o => o.Discount)
            });

        var commission = commissionDto.CategoryCommissions.Join(
            categoriesRevenues,
            cc => cc.CategoryId,
            cr => cr.CategoryId,
            (cc, cr) => cr.Amount * (cc.PercentageOfTotal / 100)).Sum();

        return commission;
    }
}
