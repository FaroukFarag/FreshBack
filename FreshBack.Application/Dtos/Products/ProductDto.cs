using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.BranchesProducts;
using FreshBack.Application.Dtos.Merchants;

namespace FreshBack.Application.Dtos.Products;

public class ProductDto : BaseModelDto<int>
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string DescriptionEn { get; set; } = default!;
    public string Allergens { get; set; } = default!;
    public string AllergensEn { get; set; } = default!;
    public decimal Price { get; set; }
    public decimal WeightInKg { get; set; }
    public int MerchantId { get; set; }

    public MerchantDto Merchant { get; set; } = default!;
    public IEnumerable<ProductImageDto> ProductImages { get; set; } = default!;
    public IEnumerable<BranchProductDto> ProductsBranches { get; set; } = default!;
}
