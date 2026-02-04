using FreshBack.Domain.Enums.Products;

namespace FreshBack.Application.Dtos.BranchesProducts;

public class CreateBranchProductDto
{
    public int BranchId { get; set; }
    public int ProductId { get; set; }
    public decimal Discount { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime ExpiryDate { get; set; }
    public int Quantity { get; set; }
    public DateTime StartDeliveryDate { get; set; }
    public DateTime EndDeliveryDate { get; set; }
    public int Views { get; set; }
    public ProductStatus Status { get; set; }
}
