using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Branches;

public class ReviewImageDto : BaseImageModelDto<int>
{
    public int ReviewId { get; set; }

    public ReviewDto Review { get; set; } = default!;
}
