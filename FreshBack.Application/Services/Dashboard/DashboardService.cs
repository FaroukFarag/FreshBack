using FreshBack.Application.Dtos.Dashboard;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Dashboard;
using FreshBack.Domain.Enums.Merchants;
using FreshBack.Domain.Interfaces.Repositories.BranchesProducts;
using FreshBack.Domain.Interfaces.Repositories.Customers;
using FreshBack.Domain.Interfaces.Repositories.Merchants;
using FreshBack.Domain.Interfaces.Repositories.Orders;
using FreshBack.Domain.Interfaces.Repositories.ProductsOrders;
using FreshBack.Domain.Models.BranchesProducts;
using FreshBack.Domain.Models.Customers;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.Orders;
using FreshBack.Domain.Models.ProductsOrders;
using FreshBack.Domain.Models.Shared;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.Dashboard;

public class DashboardService(
    IProductOrderRepository productOrderRepository,
    ICustomerRepository customerRepository,
    IOrderRepository orderRepository,
    IBranchProductRepository branchProductRepository,
    IMerchantRepository merchantRepository) : IDashboardService
{
    private readonly IProductOrderRepository _productOrderRepository = productOrderRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IBranchProductRepository _branchProductRepository = branchProductRepository;
    private readonly IMerchantRepository _merchantRepository = merchantRepository;

    public async Task<ResultDto<ActiveCustomersDto>> ActiveCustomersAsync()
    {
        try
        {
            var now = DateTime.Now;
            var (weekStart, _) = GetWeekBoundaries(now);

            var totalCustomers = await _customerRepository.GetCountAsync();

            var newThisWeek = await _customerRepository.GetCountAsync(
                new BaseSpecification<Customer>
                {
                    Criteria = c => c.CreatedAt >= weekStart
                                 && c.CreatedAt <= now
                });

            return ResultDto<ActiveCustomersDto>.CreateSuccessResult(new ActiveCustomersDto
            {
                TotalCustomers = totalCustomers,
                NewThisWeek = newThisWeek
            });
        }

        catch (Exception ex)
        {
            return ResultDto<ActiveCustomersDto>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<RegisteredMerchantsDto>> RegisteredMerchantsAsync()
    {
        try
        {
            var totalMerchants = await _merchantRepository.GetCountAsync();

            var activeMerchants = await _merchantRepository.GetCountAsync(
                new BaseSpecification<Merchant>
                {
                    Criteria = m => m.Status == MerchantStatus.Active
                });

            return ResultDto<RegisteredMerchantsDto>.CreateSuccessResult(new RegisteredMerchantsDto
            {
                TotalMerchants = totalMerchants,
                ActiveMerchants = activeMerchants
            });
        }

        catch (Exception ex)
        {
            return ResultDto<RegisteredMerchantsDto>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<RevenueDto>> TotalRevenueAsync(int? merchantId = null)
    {
        try
        {
            var now = DateTime.Now;
            var todayStart = now.Date;
            var yesterdayStart = todayStart.AddDays(-1);
            var (weekStart, lastWeekStart) = GetWeekBoundaries(now);
            var (startOfThisMonth, startOfLastMonth) = GetMonthBoundaries(now);

            var today = await _orderRepository.GetSumAsync(o => o.OrderFinalAmount,
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= todayStart
                                 && o.CreationDate <= now
                                 && (merchantId == null || o.MerchantId == merchantId)
                });

            var yesterday = await _orderRepository.GetSumAsync(o => o.OrderFinalAmount,
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= yesterdayStart
                                 && o.CreationDate < todayStart
                                 && (merchantId == null || o.MerchantId == merchantId)
                });

            var thisWeek = await _orderRepository.GetSumAsync(o => o.OrderFinalAmount,
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= weekStart
                                 && o.CreationDate <= now
                                 && (merchantId == null || o.MerchantId == merchantId)
                });

            var lastWeek = await _orderRepository.GetSumAsync(o => o.OrderFinalAmount,
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= lastWeekStart
                                 && o.CreationDate < weekStart
                                 && (merchantId == null || o.MerchantId == merchantId)
                });

            var thisMonth = await _orderRepository.GetSumAsync(o => o.OrderFinalAmount,
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= startOfThisMonth
                                 && o.CreationDate <= now
                                 && (merchantId == null || o.MerchantId == merchantId)
                });

            var lastMonth = await _orderRepository.GetSumAsync(o => o.OrderFinalAmount,
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= startOfLastMonth
                                 && o.CreationDate < startOfThisMonth
                                 && (merchantId == null || o.MerchantId == merchantId)
                });

            return ResultDto<RevenueDto>.CreateSuccessResult(new RevenueDto
            {
                Today = today,
                PercentageComparedToYesterday = CalculateChange(today, yesterday),
                ThisWeek = thisWeek,
                PercentageComparedToLastWeek = CalculateChange(thisWeek, lastWeek),
                ThisMonth = thisMonth,
                PercentageComparedToLastMonth = CalculateChange(thisMonth, lastMonth)
            });
        }

        catch (Exception ex)
        {
            return ResultDto<RevenueDto>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<ActiveOrdersDto>> ActiveOrdersAsync()
    {
        try
        {
            var now = DateTime.Now;
            var oneHourAgo = now.AddHours(-1);
            var (startOfThisMonth, _) = GetMonthBoundaries(now);
            var daysElapsed = Math.Max((now - startOfThisMonth).TotalDays, 1);

            var ordersLastHour = await _orderRepository.GetCountAsync(
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= oneHourAgo
                                 && o.CreationDate <= now
                });

            var ordersThisMonth = await _orderRepository.GetCountAsync(
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= startOfThisMonth
                                 && o.CreationDate <= now
                });

            var dailyAverage = (int)Math.Floor(ordersThisMonth / daysElapsed);

            return ResultDto<ActiveOrdersDto>.CreateSuccessResult(new ActiveOrdersDto
            {
                ActiveOrdersAverage = dailyAverage,
                NewSinceLastHour = ordersLastHour
            });
        }

        catch (Exception ex)
        {
            return ResultDto<ActiveOrdersDto>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<IEnumerable<LatestOrderDto>>> LatestOrdersAsync(int count = 10)
    {
        try
        {
            var (orders, totalCount) = await _orderRepository.GetAllPaginatedAsync(
                new PaginatedModel
                {
                    PageNumber = 1,
                    PageSize = count
                },
                o => new LatestOrderDto
                {
                    OrderId = o.Id,
                    MerchantName = o.Merchant.Name,
                    CustomerName = o.Customer.Name!,
                    Value = o.OrderFinalAmount,
                    Status = o.Status.ToString(),
                    CreationDate = o.CreationDate
                },
                new BaseSpecification<Order>
                {
                    OrderByDescending = o => o.CreationDate,
                    Includes = [o => o.Merchant, o => o.Customer]
                });

            return ResultDto<IEnumerable<LatestOrderDto>>.CreateSuccessResult(orders);
        }
        catch (Exception ex)
        {
            return ResultDto<IEnumerable<LatestOrderDto>>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<OccupancyRateDto>> OccupancyRateAsync()
    {
        try
        {
            var now = DateTime.Now;
            var (startOfThisMonth, startOfLastMonth) = GetMonthBoundaries(now);

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

            return ResultDto<OccupancyRateDto>.CreateSuccessResult(new OccupancyRateDto
            {
                ThisMonth = (int)thisMonthQty,
                PercentageComparedToLastMonth = CalculateChange(thisMonthQty, lastMonthQty)
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

            var todaysOrders = await _orderRepository.GetAllAsync(
                new BaseSpecification<Order>
                {
                    Criteria = o => o.CreationDate >= today
                                 && o.CreationDate < today.AddDays(1)
                });

            var revenueByHour = todaysOrders
                .GroupBy(o => o.CreationDate.Hour)
                .ToDictionary(g => g.Key, g => g.Sum(o => o.OrderFinalAmount));

            var points = Enumerable.Range(0, 24)
                .Select(h => new HourlySalesPoint
                {
                    Hour = h,
                    Revenue = revenueByHour.GetValueOrDefault(h, 0m)
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
                .ToDictionary(g => g.Key, g => g.Sum(o => o.OrderFinalAmount));

            string[] dayNamesAr = ["السبت", "الأحد", "الاثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة"];
            string[] dayNamesEn = ["Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];

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
                        Revenue = revenueByDate.GetValueOrDefault(date, 0m)
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
            var (startOfThisMonth, _) = GetMonthBoundaries(now);

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
            var (startOfThisMonth, _) = GetMonthBoundaries(now);

            var soldProducts = await _productOrderRepository.GetSumAsync(
                po => po.Quantity,
                new BaseSpecification<ProductOrder>
                {
                    Criteria = po => po.Order.CreationDate >= startOfThisMonth
                                  && po.Order.CreationDate <= now
                                  && (merchantId == null || po.Order.MerchantId == merchantId)
                });

            return ResultDto<decimal>.CreateSuccessResult(soldProducts);
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
            var (startOfThisMonth, startOfLastMonth) = GetMonthBoundaries(now);

            var thisMonthCount = await _customerRepository.GetCountAsync(
                new BaseSpecification<Customer>
                {
                    Criteria = c => c.CreatedAt >= startOfThisMonth
                                 && c.CreatedAt <= now
                });

            var lastMonthCount = await _customerRepository.GetCountAsync(
                new BaseSpecification<Customer>
                {
                    Criteria = c => c.CreatedAt >= startOfLastMonth
                                 && c.CreatedAt < startOfThisMonth
                });

            var growth = lastMonthCount == 0
                ? (thisMonthCount > 0 ? 100m : 0m)
                : Math.Round(((decimal)(thisMonthCount - lastMonthCount)
                              / lastMonthCount) * 100, 2);

            return ResultDto<decimal>.CreateSuccessResult(growth);
        }

        catch (Exception ex)
        {
            return ResultDto<decimal>.CreateFailResult(ex.Message);
        }
    }

    private static decimal CalculateChange(decimal current, decimal previous) =>
        previous == 0
            ? (current > 0 ? 100m : 0m)
            : Math.Round((current - previous) / previous * 100, 2);

    private static (DateTime weekStart, DateTime lastWeekStart) GetWeekBoundaries(DateTime now)
    {
        var daysFromMonday = ((int)now.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
        var weekStart = now.Date.AddDays(-daysFromMonday);
        return (weekStart, weekStart.AddDays(-7));
    }

    private static (DateTime thisMonthStart, DateTime lastMonthStart) GetMonthBoundaries(DateTime now)
    {
        var thisMonthStart = new DateTime(now.Year, now.Month, 1);
        return (thisMonthStart, thisMonthStart.AddMonths(-1));
    }
}