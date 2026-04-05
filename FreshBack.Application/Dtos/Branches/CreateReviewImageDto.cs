using FreshBack.Application.Dtos.Abstraction;

namespace FreshBack.Application.Dtos.Branches;

public class CreateReviewImageDto : BaseImageModelDto<int>
{
    public int ReviewId { get; set; }
}
