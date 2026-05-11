namespace FreshBack.Application.Dtos.Dashboard;

public class LatestOrderDto
{
    public int OrderId { get; set; }
    public string MerchantName { get; set; } = default!;
    public string CustomerName { get; set; } = default!;
    public decimal Value { get; set; }
    public string Status { get; set; } = default!;
    public DateTime CreationDate { get; set; }
}
