using FreshBack.Application.Dtos.Dashboard;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Dashboard;
using FreshBack.Domain.Interfaces.Repositories.BranchesProducts;
using FreshBack.Domain.Interfaces.Repositories.Customers;
using FreshBack.Domain.Interfaces.Repositories.Orders;
using FreshBack.Domain.Interfaces.Repositories.ProductsOrders;
using FreshBack.Domain.Models.BranchesProducts;
using FreshBack.Domain.Models.Customers;
using FreshBack.Domain.Models.Orders;
using FreshBack.Domain.Models.ProductsOrders;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.Dashboard;

public class DashboardService(
    IProductOrderRepository productOrderRepository,
    ICustomerRepository customerRepository,
    IOrderRepository orderRepository,
    IBranchProductRepository branchProductRepository) :
    IDashboardService
{
    private readonly IProductOrderRepository _productOrderRepository = productOrderRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IBranchProductRepository _branchProductRepository = branchProductRepository;

    public async Task<ResultDto<RevenueDto>> TotalRevenueAsync()
    {
        try
        {
            var now = DateTime.Now;
            var todayStart = now.Date;
            var daysFromMonday = ((int)now.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            var weekStart = now.Date.AddDays(-daysFromMonday);
            var startOfThisMonth = new DateTime(now.Year, now.Month, 1);
            var startOfLastMonth = startOfThisMonth.AddMonths(-1);

            var today = await _orderRepository.GetSumAsync(
                o => o.Price,
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= todayStart
                                 && o.CreationDate <= now
                });

            var thisWeek = await _orderRepository.GetSumAsync(
                o => o.Price,
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= weekStart
                                 && o.CreationDate <= now
                });

            var thisMonth = await _orderRepository.GetSumAsync(
                o => o.Price,
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= startOfThisMonth
                                 && o.CreationDate <= now
                });

            var lastMonth = await _orderRepository.GetSumAsync(
                o => o.Price,
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= startOfLastMonth
                                 && o.CreationDate < startOfThisMonth
                });

            decimal changePercent = lastMonth == 0
                ? (thisMonth > 0 ? 100m : 0)
                : Math.Round((thisMonth - lastMonth) / lastMonth * 100, 2);

            return ResultDto<RevenueDto>.CreateSuccessResult(new RevenueDto
            {
                Today = today,
                ThisWeek = thisWeek,
                ThisMonth = thisMonth,
                PercentageComparedToLastMonth = changePercent
            });
        }
        catch (Exception ex)
        {
            return ResultDto<RevenueDto>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<OccupancyRateDto>> OccupancyRateAsync()
    {
        try
        {
            var now = DateTime.Now;
            var startOfThisMonth = new DateTime(now.Year, now.Month, 1);
            var startOfLastMonth = startOfThisMonth.AddMonths(-1);

            var thisMonthQty = await _productOrderRepository.GetSumAsync(
                po => po.Quantity,
                new BaseSpecification<ProductOrder>
                {
                    Criteria = po => po.Order.CreationDate >= startOfThisMonth
                                  && po.Order.CreationDate <= now
                });

            var lastMonthQty = await _productOrderRepository.GetSumAsync(
                po => po.Quantity,
                new BaseSpecification<ProductOrder>
                {
                    Criteria = po => po.Order.CreationDate >= startOfLastMonth
                                  && po.Order.CreationDate < startOfThisMonth
                });

            decimal changePercent = lastMonthQty == 0
                ? (thisMonthQty > 0 ? 100m : 0)
                : Math.Round((thisMonthQty - lastMonthQty) / lastMonthQty * 100, 2);

            return ResultDto<OccupancyRateDto>.CreateSuccessResult(new OccupancyRateDto
            {
                ThisMonth = (int)thisMonthQty,
                PercentageComparedToLastMonth = changePercent
            });
        }
        catch (Exception ex)
        {
            return ResultDto<OccupancyRateDto>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<HourlySalesDto>> HourlySalesAsync()
    {
        try
        {
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);

            var todaysOrders = await _orderRepository.GetAllAsync(
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= today
                                 && o.CreationDate < tomorrow
                });

            var revenueByHour = todaysOrders
                .GroupBy(o => o.CreationDate.Hour)
                .ToDictionary(g => g.Key, g => g.Sum(o => o.Price));

            var points = Enumerable.Range(0, 24)
                .Select(h => new HourlySalesPoint
                {
                    Hour = h,
                    Revenue = revenueByHour.TryGetValue(h, out var rev) ? rev : 0m
                })
                .ToList();

            return ResultDto<HourlySalesDto>.CreateSuccessResult(new HourlySalesDto
            {
                Points = points
            });
        }
        catch (Exception ex)
        {
            return ResultDto<HourlySalesDto>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<WeeklySalesDto>> WeeklySalesAsync()
    {
        try
        {
            var today = DateTime.Now.Date;
            var daysFromSaturday = ((int)today.DayOfWeek - (int)DayOfWeek.Saturday + 7) % 7;
            var weekStart = today.AddDays(-daysFromSaturday);
            var weekEnd = weekStart.AddDays(7);

            var weekOrders = await _orderRepository.GetAllAsync(
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= weekStart
                                 && o.CreationDate < weekEnd
                });

            var revenueByDate = weekOrders
                .GroupBy(o => o.CreationDate.Date)
                .ToDictionary(g => g.Key, g => g.Sum(o => o.Price));

            var dayNamesAr = new[] { "السبت", "الأحد", "الاثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة" };
            var dayNamesEn = new[] { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

            var points = Enumerable.Range(0, 7)
                .Select(i =>
                {
                    var date = weekStart.AddDays(i);
                    return new WeeklySalesPoint
                    {
                        DayIndex = i,
                        Date = DateOnly.FromDateTime(date),
                        DayNameAr = dayNamesAr[i],
                        DayNameEn = dayNamesEn[i],
                        Revenue = revenueByDate.TryGetValue(date, out var rev) ? rev : 0m
                    };
                })
                .ToList();

            return ResultDto<WeeklySalesDto>.CreateSuccessResult(new WeeklySalesDto
            {
                Points = points
            });
        }
        catch (Exception ex)
        {
            return ResultDto<WeeklySalesDto>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<decimal>> SavedProductsWeightAsync(int? merchantId = null)
    {
        try
        {
            var now = DateTime.Now;
            var startOfThisMonth = new DateTime(now.Year, now.Month, 1);

            var productsWeight = await _productOrderRepository.GetSumAsync(
                po => po.Quantity * po.Product.WeightInKg,
                new BaseSpecification<ProductOrder>
                {
                    Criteria = po => po.Order.CreationDate >= startOfThisMonth
                                  && po.Order.CreationDate <= now
                                  && (merchantId == null || po.Product.MerchantId == merchantId)
                });

            return ResultDto<decimal>.CreateSuccessResult(productsWeight);
        }
        catch (Exception ex)
        {
            return ResultDto<decimal>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<decimal>> SoldProductsAsync(int? merchantId = null)
    {
        try
        {
            var now = DateTime.Now;
            var startOfThisMonth = new DateTime(now.Year, now.Month, 1);

            var productsWeight = await _productOrderRepository.GetSumAsync(
                po => po.Quantity,
                new BaseSpecification<ProductOrder>
                {
                    Criteria = po => po.Order.CreationDate >= startOfThisMonth
                                  && po.Order.CreationDate <= now
                                  && (merchantId == null || po.Order.MerchantId == merchantId)
                });

            return ResultDto<decimal>.CreateSuccessResult(productsWeight);
        }

        catch (Exception ex)
        {
            return ResultDto<decimal>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<decimal>> RemainingProductsAsync(int? merchantId = null)
    {
        try
        {
            var now = DateTime.Now;

            var remainingQuantity = await _branchProductRepository.GetSumAsync(
                bp => bp.Quantity,
                new BaseSpecification<BranchProduct>
                {
                    Criteria = bp => bp.ExpiryDate > now
                                  && (merchantId == null || bp.Product.MerchantId == merchantId)
                });

            return ResultDto<decimal>.CreateSuccessResult(remainingQuantity);
        }
        catch (Exception ex)
        {
            return ResultDto<decimal>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<decimal>> MonthlyGrowthPercentageAsync()
    {
        try
        {
            var now = DateTime.Now;
            var startOfThisMonth = new DateTime(now.Year, now.Month, 1);
            var startOfLastMonth = startOfThisMonth.AddMonths(-1);

            var lastMonthCustomersCount = await _customerRepository
                .GetCountAsync(new BaseSpecification<Customer>
                {
                    Criteria = c => c.CreatedAt >= startOfLastMonth &&
                                    c.CreatedAt < startOfThisMonth
                });

            var thisMonthCustomersCount = await _customerRepository
                .GetCountAsync(new BaseSpecification<Customer>
                {
                    Criteria = c => c.CreatedAt >= startOfThisMonth &&
                                    c.CreatedAt <= now
                });

            if (lastMonthCustomersCount == 0)
                return ResultDto<decimal>
                    .CreateSuccessResult(thisMonthCustomersCount > 0 ? 100 : 0);

            var growthPercentage = ((decimal)(thisMonthCustomersCount -
                lastMonthCustomersCount) / lastMonthCustomersCount) * 100;

            return ResultDto<decimal>
                .CreateSuccessResult(Math.Round(growthPercentage, 2));
        }

        catch (Exception ex)
        {
            return ResultDto<decimal>.CreateFailResult(ex.Message);
        }
    }
}
