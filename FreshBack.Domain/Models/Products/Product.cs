using FreshBack.Domain.Enums.Products;
using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Merchants;

namespace FreshBack.Domain.Models.Products;

public class Product : BaseModel<int>
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Discount { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public ProductStatus Status { get; set; }
    public int MerchantId { get; set; }

    public Merchant Merchant { get; set; } = default!;
}
