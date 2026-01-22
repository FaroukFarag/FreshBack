using FreshBack.Common.Extensions;
using FreshBack.Domain.Enums.Shared;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using System.Linq.Expressions;

namespace FreshBack.Domain.Specifications.Absraction;

public class BaseSpecification<TEntity> : IBaseSpecification<TEntity>
{
    public Expression<Func<TEntity, bool>>? Criteria { get; set; }
    public List<Expression<Func<TEntity, object>>> Includes { get; set; } = [];
    public List<IncludeChain<TEntity>> IncludeChains { get; set; } = [];
    public Expression<Func<TEntity, object>>? OrderBy { get; set; }
    public Expression<Func<TEntity, object>>? OrderByDescending { get; set; }

    protected void AddCriteria(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    public void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    public void AddIncludeChain(IncludeChain<TEntity> includeChain)
    {
        IncludeChains.Add(includeChain);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderByDescending = null;
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
    {
        OrderBy = null;
        OrderByDescending = orderByDescendingExpression;
    }

    protected void ApplyOrder<TKey>(
        Expression<Func<TEntity, TKey>> expr,
        SortDirection direction)
    {
        if (direction == SortDirection.Asc)
            AddOrderBy(ToObject(expr));
        else
            AddOrderByDescending(ToObject(expr));
    }

    private static Expression<Func<TEntity, object>> ToObject<TKey>(
        Expression<Func<TEntity, TKey>> expr)
    {
        return Expression.Lambda<Func<TEntity, object>>(
            Expression.Convert(expr.Body, typeof(object)),
            expr.Parameters);
    }

}

public static class IncludeChainExtensions
{
    public static IncludeChain<TEntity> CreateIncludeChain<TEntity, TProperty>(
        this BaseSpecification<TEntity> spec,
        Expression<Func<TEntity, TProperty>> initialInclude)
    {
        var chain = new IncludeChain<TEntity>
        {
            InitialInclude = initialInclude as Expression<Func<TEntity, object>> ??
                           Expression.Lambda<Func<TEntity, object>>(
                               Expression.Convert(initialInclude.Body, typeof(object)),
                               initialInclude.Parameters)
        };

        spec.AddIncludeChain(chain);

        return chain;
    }

    public static IncludeChain<TEntity> ThenInclude<TEntity, TProperty>(
        this IncludeChain<TEntity> chain,
        Expression<Func<object, TProperty>> thenInclude)
    {
        var convertedExpression = thenInclude as Expression<Func<object, object>> ??
            Expression.Lambda<Func<object, object>>(
                Expression.Convert(thenInclude.Body, typeof(object)),
                thenInclude.Parameters);

        chain.ThenIncludes.Add(convertedExpression);

        return chain;
    }
}
