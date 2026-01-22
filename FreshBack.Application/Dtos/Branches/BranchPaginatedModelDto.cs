using FreshBack.Application.Dtos.Shared;
using FreshBack.Domain.Enums.Branches;
using FreshBack.Domain.Enums.Shared;

namespace FreshBack.Application.Dtos.Branches;

public class BranchPaginatedModelDto : PaginatedModelDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public BranchSortBy SortBy { get; set; } = BranchSortBy.Distance;
    public SortDirection SortDirection { get; set; } = SortDirection.Asc;
}
