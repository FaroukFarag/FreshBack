using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.BranchesProducts;
using FreshBack.Domain.Models.Carts;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.ProductsOrders;

namespace FreshBack.Domain.Models.Products;

public class Product : BaseModel<int>
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

    public Merchant Merchant { get; set; } = default!;
    public IEnumerable<ProductImage> ProductImages { get; set; } = default!;
    public IEnumerable<CartItem> CartItems { get; set; } = default!;
    public IEnumerable<ProductOrder> ProductsOrders { get; set; } = default!;
    public IEnumerable<BranchProduct> ProductsBranches { get; set; } = default!;
}
