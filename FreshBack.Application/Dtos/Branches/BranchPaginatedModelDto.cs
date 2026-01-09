using FreshBack.Application.Dtos.Shared;

namespace FreshBack.Application.Dtos.Branches;

public class BranchPaginatedModelDto : PaginatedModelDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
