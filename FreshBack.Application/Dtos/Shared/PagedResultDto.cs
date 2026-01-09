namespace FreshBack.Application.Dtos.Shared;

public class PagedResult<T>(IEnumerable<T> items, int totalCount)
{
    public IReadOnlyCollection<T> Items { get; } = items.ToList().AsReadOnly();
    public int TotalCount { get; } = totalCount;
}
