using FreshBack.Common.Extensions;
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

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    public void AddIncludeChain(IncludeChain<TEntity> includeChain)
    {
        IncludeChains.Add(includeChain);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
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
