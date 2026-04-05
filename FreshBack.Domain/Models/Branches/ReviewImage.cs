using FreshBack.Domain.Models.Abstraction;

namespace FreshBack.Domain.Models.Branches;

public class ReviewImage : BaseImageAuditModel<int>
{
    public int ReviewId { get; set; }

    public Review Review { get; set; } = default!;
}
