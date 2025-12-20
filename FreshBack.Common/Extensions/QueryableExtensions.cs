using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FreshBack.Common.Extensions;

public class IncludeChain<TEntity>
{
    public Expression<Func<TEntity, object>> InitialInclude { get; set; } = default!;
    public List<Expression<Func<object, object>>> ThenIncludes { get; set; } = [];

    public IncludeChain<TEntity> ThenInclude(Expression<Func<object, object>> thenInclude)
    {
        ThenIncludes.Add(thenInclude);
        return this;
    }
}

public static class QueryableExtensions
{
    public static IQueryable<TEntity> ApplyCriteria<TEntity>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, bool>>? criteria)
        where TEntity : class
    {
        return criteria != null ? query.Where(criteria) : query;
    }

    public static IQueryable<TEntity> ApplyIncludes<TEntity>(
        this IQueryable<TEntity> query,
        List<Expression<Func<TEntity, object>>>? includes)
        where TEntity : class
    {
        if (includes == null)
            return query;

        return includes.Aggregate(query, (current, include) => current.Include(include));
    }

    public static IQueryable<TEntity> ApplyIncludeChains<TEntity>(
        this IQueryable<TEntity> query,
        List<IncludeChain<TEntity>>? includeChains)
        where TEntity : class
    {
        if (includeChains == null || includeChains.Count == 0)
            return query;

        foreach (var chain in includeChains)
        {
            var includableQuery = query.Include(chain.InitialInclude);

            foreach (var thenInclude in chain.ThenIncludes)
            {
                includableQuery = includableQuery.ThenInclude(thenInclude);
            }

            query = includableQuery;
        }

        return query;
    }

    public static IQueryable<TEntity> ApplyOrderBy<TEntity>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, object>>? orderBy)
        where TEntity : class
    {
        return orderBy != null ? query.OrderBy(orderBy) : query;
    }

    public static IQueryable<TEntity> ApplyOrderByDescending<TEntity>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, object>>? orderByDescending)
        where TEntity : class
    {
        return orderByDescending != null ? query.OrderByDescending(orderByDescending) : query;
    }
}
