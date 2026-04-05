using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Dashboard;
using FreshBack.Domain.Interfaces.Repositories.Customers;
using FreshBack.Domain.Interfaces.Repositories.ProductsOrders;
using FreshBack.Domain.Models.Customers;
using FreshBack.Domain.Models.ProductsOrders;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.Dashboard;

public class DashboardService(
    IProductOrderRepository productOrderRepository,
    ICustomerRepository customerRepository) :
    IDashboardService
{
    private readonly IProductOrderRepository _productOrderRepository = productOrderRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<ResultDto<decimal>> SavedProductsWeightAsync()
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
                });

            return ResultDto<decimal>.CreateSuccessResult(productsWeight);
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
