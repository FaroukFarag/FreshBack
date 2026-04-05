using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Branches;

public class CreateReviewDto : BaseModelDto<int>
{
    public int? Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public int CustomerId { get; set; }
    public int MerchantId { get; set; }
    public int BranchId { get; set; }

    public IEnumerable<CreateReviewImageDto> ReviewImages { get; set; } = default!;
}
