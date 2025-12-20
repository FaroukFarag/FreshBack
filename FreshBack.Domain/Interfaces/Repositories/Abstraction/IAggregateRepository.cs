using FreshBack.Domain.Interfaces.Specifications.Absraction;
using System.Linq.Expressions;

namespace FreshBack.Domain.Interfaces.Repositories.Abstraction;

public interface IAggregateRepository<TEntity> where TEntity : class
{
    Task<long> GetCountAsync(IBaseSpecification<TEntity>? spec = null);

    Task<decimal> GetSumAsync(
        Expression<Func<TEntity, decimal>> selector,
        IBaseSpecification<TEntity>? spec = null);

    Task<decimal> GetAverageAsync(
        Expression<Func<TEntity, decimal>> selector,
        IBaseSpecification<TEntity>? spec = null);

    Task<TResult> GetMaxAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null);

    Task<TResult> GetMinAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null);
}
