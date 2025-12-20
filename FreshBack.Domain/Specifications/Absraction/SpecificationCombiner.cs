using FreshBack.Domain.Interfaces.Specifications.Absraction;
using System.Linq.Expressions;

namespace FreshBack.Domain.Specifications.Absraction;

public class SpecificationCombiner<TEntity> : ISpecificationCombiner<TEntity>
{
    public IBaseSpecification<TEntity> Combine(
        IBaseSpecification<TEntity>? spec,
        Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate == null)
            return spec ?? new BaseSpecification<TEntity>();

        var combinedCriteria = SpecificationCombiner<TEntity>.CombineCriteria(spec?.Criteria, predicate);

        return new BaseSpecification<TEntity>
        {
            Criteria = combinedCriteria,
            Includes = spec?.Includes ?? [],
            OrderBy = spec?.OrderBy,
            OrderByDescending = spec?.OrderByDescending
        };
    }

    private static Expression<Func<TEntity, bool>> CombineCriteria(
        Expression<Func<TEntity, bool>>? criteria,
        Expression<Func<TEntity, bool>> predicate)
    {
        if (criteria == null)
            return predicate;

        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(criteria.Body, Expression.Invoke(predicate, criteria.Parameters)),
            criteria.Parameters);
    }
}
