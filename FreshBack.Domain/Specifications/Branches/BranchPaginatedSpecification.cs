using FreshBack.Common.Extensions;
using FreshBack.Common.Utilities;
using FreshBack.Domain.Enums.Branches;
using FreshBack.Domain.Enums.Shared;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.BranchesProducts;
using FreshBack.Domain.Specifications.Absraction;
using NetTopologySuite.Geometries;

namespace FreshBack.Domain.Specifications.Branches;

public sealed class BranchPaginatedSpecification : BaseSpecification<Branch>
{
    public BranchPaginatedSpecification(
        string? searchTerm,
        int? categoryId,
        BranchSortBy sortBy,
        SortDirection sortDirection,
        Point userLocation)
    {
        ApplyCriteria(searchTerm, categoryId);
        ApplyIncludes();
        ApplySorting(sortBy, sortDirection, userLocation);
    }

    private void ApplyCriteria(string? searchTerm, int? categoryId)
    {
        searchTerm = searchTerm?.Trim();

        Criteria = b =>
            b.Status == BranchStatus.Active &&
            (
                string.IsNullOrEmpty(searchTerm) ||
                b.Name.Contains(searchTerm) ||
                b.NameEn.Contains(searchTerm) ||
                b.BranchesProducts.Any(bp => bp.Product.Name.Contains(searchTerm))
            );

        if (categoryId.HasValue)
        {
            Criteria = Criteria!.And(
                b => b.CategoryId == categoryId);
        }
    }

    private void ApplyIncludes()
    {
        AddInclude(b => b.Merchant);
        AddIncludeChain(new IncludeChain<Branch>
        {
            InitialInclude = b => b.BranchesProducts,
            ThenIncludes =
            [
                bp => (bp as BranchProduct)!.Product
            ]
        });
    }

    private void ApplySorting(
        BranchSortBy sortBy,
        SortDirection direction,
        Point userLocation)
    {
        switch (sortBy)
        {
            case BranchSortBy.Price:
                ApplyOrder(b => b.BranchesProducts.Min(p => p.Product.Price), direction);
                break;

            case BranchSortBy.Category:
                ApplyOrder(b => b.Category.Name, direction);
                break;

            case BranchSortBy.Distance:
            default:
                ApplyOrder(b => b.Location.Distance(userLocation), direction);
                break;
        }
    }
}
