namespace FreshBack.Application.Dtos.Branches;

public class CreateReviewDto
{
    public int Rating { get; set; }
    public string Comment { get; set; } = default!;
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public int MerchantId { get; set; }
    public int BranchId { get; set; }
}
