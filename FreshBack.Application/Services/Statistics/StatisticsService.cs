using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Dtos.Statistics;
using FreshBack.Application.Interfaces.Statistics;
using FreshBack.Domain.Interfaces.Repositories.Merchants;
using FreshBack.Domain.Interfaces.Repositories.Products;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.Products;
using FreshBack.Domain.Models.Shared;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.Statistics;

public class StatisticsService(
    IMerchantRepository merchantRepository,
    IProductRepository productRepository) :
    IStatisticsService
{
    private readonly IMerchantRepository _merchantRepository = merchantRepository;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<ResultDto<IEnumerable<TopMerchantDto>>> GetTopTenMerchants()
    {
        try
        {
            var paginatedModel = new PaginatedModel
            {
                PageNumber = 1,
                PageSize = 10
            };
            var spec = new BaseSpecification<Merchant>
            {
                OrderByDescending = m => m.Branches.SelectMany(b => b.Orders
                            .SelectMany(o => o.ProductsOrders))
                            .Sum(m => m.Quantity * (m.Product.Price -
                                m.Product.Discount)) -
                        m.Branches.SelectMany(b => b.Orders).Sum(o => o.Discount)
            };

            var (topTenMerchants, totalCount) = await _merchantRepository
                .GetAllPaginatedAsync(
                    paginatedModel,
                    m => new TopMerchantDto
                    {
                        MerchantName = m.Name,
                        TotalSales = m.Branches.SelectMany(b => b.Orders
                                .SelectMany(o => o.ProductsOrders))
                                .Sum(m => m.Quantity * (m.Product.Price -
                                    m.Product.Discount)) -
                            m.Branches.SelectMany(b => b.Orders).Sum(o => o.Discount)

                    }, spec);

            return ResultDto<IEnumerable<TopMerchantDto>>
                .CreateSuccessResult(topTenMerchants);
        }

        catch (Exception ex)
        {
            return ResultDto<IEnumerable<TopMerchantDto>>.CreateFailResult(ex.Message);
        }
    }

    public async Task<ResultDto<IEnumerable<TopProductDto>>> GetTopTenProducts()
    {
        try
        {
            var paginatedModel = new PaginatedModel
            {
                PageNumber = 1,
                PageSize = 10
            };
            var spec = new BaseSpecification<Product>
            {
                OrderByDescending = p => p.ProductsOrders.Count()
            };

            var (topTenProducts, totalCount) = await _productRepository
                .GetAllPaginatedAsync(
                    paginatedModel,
                    p => new TopProductDto
                    {
                        ProductName = p.Name,
                        Count = p.ProductsOrders.Count()
                    }, spec);

            return ResultDto<IEnumerable<TopProductDto>>
                .CreateSuccessResult(topTenProducts);
        }

        catch (Exception ex)
        {
            return ResultDto<IEnumerable<TopProductDto>>.CreateFailResult(ex.Message);
        }
    }
}
