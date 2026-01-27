using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Shared;
using System.Linq.Expressions;

namespace FreshBack.Domain.Interfaces.Repositories.Abstraction;

public interface IReadRepository<TEntity, TPrimaryKey> where TEntity : class
{
    Task<TEntity> GetAsync(
        TPrimaryKey id,
        IBaseSpecification<TEntity>? spec = null);

    Task<TResult> GetAsync<TResult>(
        TPrimaryKey id,
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null);

    Task<IEnumerable<TEntity>> GetAllAsync(IBaseSpecification<TEntity>? spec = null);

    Task<IEnumerable<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null);

    Task<(IEnumerable<TEntity>, int)> GetAllPaginatedAsync(
        PaginatedModel paginatedModel,
        IBaseSpecification<TEntity>? spec = null);

    Task<(IEnumerable<TResult>, int)> GetAllPaginatedAsync<TResult>(
        PaginatedModel paginatedModel,
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null);

    Task<IEnumerable<TEntity>> GetAllFilteredAsync<TFilterDto>(
        TFilterDto filterDto,
        IBaseSpecification<TEntity>? spec = null);
}
