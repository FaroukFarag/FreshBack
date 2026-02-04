using FreshBack.Domain.Enums.Products;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Products;
using FreshBack.Domain.Shared.Attributs;

namespace FreshBack.Domain.Models.BranchesProducts;

public class BranchProduct
{
    [CompositeKey]
    public int BranchId { get; set; }

    [CompositeKey]
    public int ProductId { get; set; }

    public decimal Discount { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime ExpiryDate { get; set; }
    public int Quantity { get; set; }
    public DateTime StartDeliveryDate { get; set; }
    public DateTime EndDeliveryDate { get; set; }
    public int Views { get; set; }
    public ProductStatus Status { get; set; }

    public Branch Branch { get; set; } = default!;
    public Product Product { get; set; } = default!;
}
