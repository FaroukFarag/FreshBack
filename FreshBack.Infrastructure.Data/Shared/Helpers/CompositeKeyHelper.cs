using FreshBack.Domain.Shared.Attributs;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FreshBack.Infrastructure.Data.Shared.Helpers;

public static class CompositeKeyHelper
{
    public static Expression<Func<TEntity, bool>> BuildCompositeKeyPredicate<TEntity, TKey>(TKey key)
    {
        var entityType = typeof(TEntity);
        var parameter = Expression.Parameter(entityType, "e");
        Expression? combinedExpression = null;

        if (key is ITuple tuple)
        {
            var keyProperties = entityType.GetProperties()
                .Where(p => p.GetCustomAttribute<CompositeKeyAttribute>() != null)
                .OrderBy(p => p.Name)
                .ToList();

            if (keyProperties.Count != tuple.Length)
                throw new ArgumentException("Key tuple length doesn't match composite key properties");

            for (int i = 0; i < tuple.Length; i++)
            {
                var property = Expression.Property(parameter, keyProperties[i]);
                var constant = Expression.Constant(tuple[i]);
                var equality = Expression.Equal(property, constant);

                combinedExpression = combinedExpression == null
                    ? equality
                    : Expression.AndAlso(combinedExpression, equality);
            }
        }

        if (combinedExpression == null)
            throw new InvalidOperationException("Could not build composite key predicate");

        return Expression.Lambda<Func<TEntity, bool>>(combinedExpression, parameter);
    }

    public static object[] GetCompositeKeyValues<TEntity>(TEntity entity)
    {
        return typeof(TEntity)
            .GetProperties()
            .Where(p => p.GetCustomAttribute<CompositeKeyAttribute>() != null)
            .OrderBy(p => p.Name)
            .Select(p => p.GetValue(entity))
            .ToArray()!;
    }
}
