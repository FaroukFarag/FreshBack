using FreshBack.Application.Dtos.Shared;

namespace FreshBack.Application.Dtos.Branches;

public class ReviewsForBranchPaginatedDto : PaginatedModelDto
{
    public int BranchId { get; set; }
}
