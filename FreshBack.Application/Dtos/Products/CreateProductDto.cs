using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Domain.Enums.Products;

namespace FreshBack.Application.Dtos.Products;

public class CreateProductDto : BaseModelDto<int>
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Allergens { get; set; } = default!;
    public string Warnings { get; set; } = default!;
    public decimal Discount { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int Views { get; set; }
    public DateTime StartDeliveryDate { get; set; }
    public DateTime EndDeliveryDate { get; set; }
    public ProductStatus Status { get; set; }
    public int MerchantId { get; set; }
    public int BranchId { get; set; }

    public IEnumerable<CreateProductImageDto> ProductImages { get; set; } = default!;
}
