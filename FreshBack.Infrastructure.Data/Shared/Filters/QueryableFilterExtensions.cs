using FreshBack.Common.Utilities;
using FreshBack.Domain.Shared.Attributs;
using System.Linq.Expressions;
using System.Reflection;

namespace FreshBack.Infrastructure.Data.Shared.Filters;

public static class QueryableFilterExtensions
{
    private static readonly Dictionary<Type, Dictionary<string, Func<Expression, Expression, Expression>>> FilterConditionsCache = [];

    public static Expression<Func<TEntity, bool>> ToPredicate<TEntity, TFilterDto>(this TFilterDto filterDto)
    {
        var predicate = PredicateBuilder.True<TEntity>();
        var filterConditions = GetFilterConditions<TFilterDto>();

        foreach (var filterProperty in GetNonNullProperties(filterDto))
        {
            var entityPropertyName = filterProperty.GetCustomAttribute<FilterPropertyAttribute>()?.EntityPropertyName ?? filterProperty.Name;

            if (typeof(TEntity).GetProperty(entityPropertyName) is not { } entityProperty) continue;

            var parameter = Expression.Parameter(typeof(TEntity), "t");
            var propertyAccess = Expression.Property(parameter, entityProperty);
            var constant = Expression.Constant(filterProperty.GetValue(filterDto), entityProperty.PropertyType);
            var converted = Expression.Convert(constant, entityProperty.PropertyType);

            var condition = filterConditions.TryGetValue(filterProperty.Name, out var filterExpression)
                ? filterExpression(propertyAccess, converted)
                : Expression.Equal(propertyAccess, converted);

            predicate = predicate.And(Expression.Lambda<Func<TEntity, bool>>(condition, parameter));
        }

        return predicate;
    }

    private static IEnumerable<PropertyInfo> GetNonNullProperties<TFilterDto>(TFilterDto filterDto) =>
        typeof(TFilterDto).GetProperties().Where(p => p.GetValue(filterDto) != null);

    private static Dictionary<string, Func<Expression, Expression, Expression>> GetFilterConditions<TFilterDto>()
    {
        var type = typeof(TFilterDto);

        if (FilterConditionsCache.TryGetValue(type, out var cachedConditions)) return cachedConditions;

        var conditions = type.GetProperties()
            .Where(p => p.Name.StartsWith("From") || p.Name.StartsWith("To"))
            .ToDictionary(
                p => p.Name,
                p => p.Name.StartsWith("From")
                    ? (Func<Expression, Expression, Expression>)((left, right) => Expression.GreaterThanOrEqual(left, right))
                    : (left, right) => Expression.LessThanOrEqual(left, right)
            );

        return FilterConditionsCache[type] = conditions;
    }
}
