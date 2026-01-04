using FreshBack.Domain.Enums.Products;
using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.ProductsOrders;

namespace FreshBack.Domain.Models.Products;

public class Product : BaseModel<int>
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

    public Merchant Merchant { get; set; } = default!;
    public IEnumerable<ProductImage> ProductImages { get; set; } = default!;
    public IEnumerable<ProductOrder> ProductsOrders { get; set; } = default!;
}
