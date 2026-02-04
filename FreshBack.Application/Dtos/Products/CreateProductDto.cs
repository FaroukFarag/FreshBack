using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.BranchesProducts;

namespace FreshBack.Application.Dtos.Products;

public class CreateProductDto : BaseModelDto<int>
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string DescriptionEn { get; set; } = default!;
    public string Allergens { get; set; } = default!;
    public string AllergensEn { get; set; } = default!;
    public string Warnings { get; set; } = default!;
    public string WarningsEn { get; set; } = default!;
    public decimal Price { get; set; }
    public decimal WeightInKg { get; set; }
    public int MerchantId { get; set; }

    public IEnumerable<CreateProductImageDto> ProductImages { get; set; } = default!;
    public IEnumerable<CreateBranchProductDto> ProductsBranches { get; set; } = default!;
}
