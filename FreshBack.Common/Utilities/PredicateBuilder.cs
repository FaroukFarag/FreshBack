using System.Linq.Expressions;

namespace FreshBack.Common.Utilities;

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> True<T>() => t => true;

    public static Expression<Func<T, bool>> False<T>() => t => false;

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        var parameter = first.Parameters[0];
        var visitor = new ReplaceExpressionVisitor(second.Parameters[0], parameter);
        var body = Expression.AndAlso(first.Body, visitor.Visit(second.Body));

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private class ReplaceExpressionVisitor(Expression oldValue, Expression newValue) : ExpressionVisitor
    {
        private readonly Expression _oldValue = oldValue;
        private readonly Expression _newValue = newValue;

        protected override Expression VisitParameter(ParameterExpression node)
            => node == _oldValue ? _newValue : base.VisitParameter(node);
    }
}
