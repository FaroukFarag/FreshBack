using FreshBack.Application.Dtos.FinanceManagement;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.FinanceManagement;
using FreshBack.Domain.Interfaces.Repositories.Merchants;
using FreshBack.Domain.Interfaces.Repositories.Orders;
using FreshBack.Domain.Interfaces.Repositories.ProductsOrders;
using FreshBack.Domain.Interfaces.Repositories.Settings.Areas;
using FreshBack.Domain.Models.Orders;
using FreshBack.Domain.Models.ProductsOrders;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.FinanceManagement;

public class FinanceManagementService(
    IOrderRepository orderRepository,
    IProductOrderRepository productOrderRepository,
    IAreaRepository areaRepository,
    IMerchantRepository merchantRepository) :
    IFinanceManagementService
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IProductOrderRepository _productOrderRepository =
        productOrderRepository;
    private readonly IAreaRepository _areaRepository = areaRepository;
    private readonly IMerchantRepository _merchantRepository = merchantRepository;

    public async Task<ResultDto<TotalRevenuesDto>> GetTotalRevenues()
    {
        try
        {
            var thisMonth = GetMonthRange(0);
            var lastMonth = GetMonthRange(-1);

            var totalRevenue =
                await _productOrderRepository.GetSumAsync(
                    po => po.Quantity * (po.Product.Price - po.Product.Discount))
                - await _orderRepository.GetSumAsync(o => o.Discount);

            var thisMonthRevenue =
                await GetRevenueAsync(thisMonth.fromDate, thisMonth.toDate);

            var lastMonthRevenue =
                await GetRevenueAsync(lastMonth.fromDate, lastMonth.toDate);

            return ResultDto<TotalRevenuesDto>.CreateSuccessResult(
                new TotalRevenuesDto
                {
                    Amount = totalRevenue,
                    PercentageComparedToLastMonth =
                        Math.Round(
                            CalculatePercentage(thisMonthRevenue, lastMonthRevenue),
                            2)
                });
        }

        catch (Exception ex)
        {
            return ResultDto<TotalRevenuesDto>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<OrderAverageValueDto>> GetOrderAverageValue()
    {
        try
        {
            var thisMonth = GetMonthRange(0);
            var lastMonth = GetMonthRange(-1);

            var thisMonthRevenue =
                await GetRevenueAsync(thisMonth.fromDate, thisMonth.toDate);
            var lastMonthRevenue =
                await GetRevenueAsync(lastMonth.fromDate, lastMonth.toDate);

            var thisMonthOrders =
                await GetOrderCountAsync(thisMonth.fromDate, thisMonth.toDate);
            var lastMonthOrders =
                await GetOrderCountAsync(lastMonth.fromDate, lastMonth.toDate);

            var thisMonthAov =
                thisMonthOrders == 0 ? 0 : thisMonthRevenue / thisMonthOrders;
            var lastMonthAov =
                lastMonthOrders == 0 ? 0 : lastMonthRevenue / lastMonthOrders;

            return ResultDto<OrderAverageValueDto>.CreateSuccessResult(
                new OrderAverageValueDto
                {
                    Amount = Math.Round(thisMonthAov, 2),
                    PercentageComparedToLastMonth =
                        Math.Round(
                            CalculatePercentage(thisMonthAov, lastMonthAov),
                            2)
                });
        }

        catch (Exception ex)
        {
            return ResultDto<OrderAverageValueDto>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<IEnumerable<TotalAreaRevenuesDto>>>
        GetTotalRevenuesByArea(int? month)
    {
        try
        {
            var areaRevenues = await _areaRepository.GetAllAsync(a =>
                new TotalAreaRevenuesDto
                {
                    AreaName = a.Name,
                    Amount =
                        a.Branches
                         .SelectMany(b => b.Orders)
                         .Where(o => !month.HasValue || o.CreationDate.Month == month.Value)
                         .SelectMany(o => o.ProductsOrders,
                             (o, po) => new
                             {
                                 Revenue =
                                     po.Quantity * (po.Product.Price - po.Product.Discount)
                             })
                         .Sum(ar => ar.Revenue)
                        -
                        a.Branches
                         .SelectMany(b => b.Orders)
                         .Where(o => !month.HasValue || o.CreationDate.Month == month.Value)
                         .Sum(o => o.Discount)
                });

            return ResultDto<IEnumerable<TotalAreaRevenuesDto>>
                .CreateSuccessResult(areaRevenues);
        }
        catch (Exception ex)
        {
            return ResultDto<IEnumerable<TotalAreaRevenuesDto>>
                .CreateFailResult(ex.Message);
        }
    }


    public async Task<ResultDto<IEnumerable<TotalMerchantRevenuesDto>>>
        GetTotalRevenuesByMerchant()
    {
        try
        {
            var areaRevenues = await _merchantRepository
                .GetAllAsync(m => new TotalMerchantRevenuesDto
                {
                    MerchantName = m.Name,
                    Amount = m.Branches.SelectMany(b => b.Orders
                            .SelectMany(o => o.ProductsOrders, (o, po) => new
                            {
                                Revenue = po.Quantity * (po.Product.Price - po.Product.Discount)
                            })).Sum(mr => mr.Revenue) -
                        m.Branches.SelectMany(b => b.Orders)
                            .Sum(o => o.Discount)
                });

            return ResultDto<IEnumerable<TotalMerchantRevenuesDto>>
                .CreateSuccessResult(areaRevenues);
        }

        catch (Exception ex)
        {
            return ResultDto<IEnumerable<TotalMerchantRevenuesDto>>
                .CreateFailResult(ex.Message);
        }
    }

    private static (DateTime fromDate, DateTime toDate) GetMonthRange(int offset)
    {
        var now = DateTime.Now;
        var start = new DateTime(now.Year, now.Month, 1).AddMonths(offset);

        return (start, start.AddMonths(1));
    }

    private async Task<decimal> GetRevenueAsync(DateTime from, DateTime to)
    {
        var productsRevenue =
            await _productOrderRepository.GetSumAsync(
                po => po.Quantity * (po.Product.Price - po.Product.Discount),
                new BaseSpecification<ProductOrder>
                {
                    Criteria = po =>
                        po.Order.CreationDate >= from &&
                        po.Order.CreationDate < to
                });

        var discounts =
            await _orderRepository.GetSumAsync(
                o => o.Discount,
                new BaseSpecification<Order>
                {
                    Criteria = o =>
                        o.CreationDate >= from &&
                        o.CreationDate < to
                });

        return productsRevenue - discounts;
    }

    private async Task<long> GetOrderCountAsync(DateTime from, DateTime to)
    {
        return await _orderRepository.GetCountAsync(
            new BaseSpecification<Order>
            {
                Criteria = o =>
                    o.CreationDate >= from &&
                    o.CreationDate < to
            });
    }

    private static decimal CalculatePercentage(decimal current, decimal previous)
    {
        return previous == 0
            ? (current > 0 ? 100 : 0)
            : ((current - previous) / previous) * 100;
    }
}
