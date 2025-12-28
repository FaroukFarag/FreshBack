using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Domain.Enums.Products;

namespace FreshBack.Application.Dtos.Products;

public class ProductDto : BaseModelDto<int>
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Discount { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public ProductStatus Status { get; set; }
    public int MerchantId { get; set; }

    public MerchantDto Merchant { get; set; } = default!;
}
