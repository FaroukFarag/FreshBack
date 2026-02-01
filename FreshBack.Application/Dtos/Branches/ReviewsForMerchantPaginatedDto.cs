using FreshBack.Application.Dtos.Shared;

namespace FreshBack.Application.Dtos.Branches;

public class ReviewsForMerchantPaginatedDto : PaginatedModelDto
{
    public int MerchantId { get; set; }
}
