using FreshBack.Common.Extensions;
using System.Linq.Expressions;

namespace FreshBack.Domain.Interfaces.Specifications.Absraction;

public interface IBaseSpecification<TEntity>
{
    Expression<Func<TEntity, bool>>? Criteria { get; }
    List<Expression<Func<TEntity, object>>> Includes { get; }
    List<IncludeChain<TEntity>> IncludeChains { get; }
    Expression<Func<TEntity, object>>? OrderBy { get; }
    Expression<Func<TEntity, object>>? OrderByDescending { get; }
}
