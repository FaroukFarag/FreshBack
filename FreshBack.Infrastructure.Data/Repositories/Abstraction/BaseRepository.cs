using FreshBack.Common.Extensions;
using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.Shared;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Shared.Filters;
using FreshBack.Infrastructure.Data.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace FreshBack.Infrastructure.Data.Repositories.Abstraction;

public class BaseRepository<TEntity, TPrimaryKey>(
    FreshBackDbContext context,
    ISpecificationCombiner<TEntity> specificationCombiner)
    : IBaseRepository<TEntity, TPrimaryKey>
    where TEntity : class
{
    private readonly FreshBackDbContext _context = context;
    private readonly ISpecificationCombiner<TEntity> _specificationCombiner = specificationCombiner;

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);

        return entity;
    }

    public virtual async Task CreateRangeAsync(IEnumerable<TEntity> entities)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities);
    }

    public virtual async Task<TEntity> GetAsync(TPrimaryKey id, IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        if (id is ITuple)
        {
            var predicate = CompositeKeyHelper.BuildCompositeKeyPredicate<TEntity, TPrimaryKey>(id);
            return await query.AsNoTracking().FirstOrDefaultAsync(predicate)
                ?? throw new ArgumentException($"Entity with id {id} not found.");
        }

        else
        {
            return await query.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<TPrimaryKey>(e, "Id")!.Equals(id))
                ?? throw new ArgumentException($"Entity with id {id} not found.");
        }
    }

    public virtual async Task<TResult> GetAsync<TResult>(
        TPrimaryKey id,
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec)
            .AsNoTracking();

        if (id is ITuple)
        {
            var predicate =
                CompositeKeyHelper.BuildCompositeKeyPredicate<TEntity, TPrimaryKey>(id);

            return await query
                .Where(predicate)
                .Select(selector)
                .FirstOrDefaultAsync()
                ?? throw new ArgumentException($"Entity with id {id} not found.");
        }

        else
        {
            return await query
                .Where(e => EF.Property<TPrimaryKey>(e, "Id")!.Equals(id))
                .Select(selector)
                .FirstOrDefaultAsync()
                ?? throw new ArgumentException($"Entity with id {id} not found.");
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        return await query.AsNoTracking().ToListAsync();
    }

    public virtual async Task<IEnumerable<TResult>>
        GetAllAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        return await query.AsNoTracking().Select(selector).ToListAsync();
    }

    public virtual async Task<(IEnumerable<TEntity>, int)> GetAllPaginatedAsync(
        PaginatedModel paginatedModel,
        IBaseSpecification<TEntity>? spec = null)
    {
        paginatedModel.PageNumber = paginatedModel.PageNumber <= 0 ? 1 : paginatedModel.PageNumber;
        var query = ApplySpecification(spec);
        var totalCount = query.Count();
        var items = await query
            .Skip((paginatedModel.PageNumber - 1) * paginatedModel.PageSize)
            .Take(paginatedModel.PageSize)
            .AsNoTracking()
            .ToListAsync();

        return (items, totalCount);
    }

    public virtual async Task<(IEnumerable<TResult>, int)> GetAllPaginatedAsync<TResult>(
        PaginatedModel paginatedModel,
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        paginatedModel.PageNumber = paginatedModel.PageNumber <= 0 ? 1 : paginatedModel.PageNumber;

        var query = ApplySpecification(spec);
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((paginatedModel.PageNumber - 1) * paginatedModel.PageSize)
            .Take(paginatedModel.PageSize)
            .Select(selector)
            .ToListAsync();

        return (items, totalCount);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllFilteredAsync<TFilterDto>(
        TFilterDto filterDto,
        IBaseSpecification<TEntity>? spec = null)
    {
        if (filterDto == null)
            throw new ArgumentNullException(nameof(filterDto));

        var predicate = filterDto.ToPredicate<TEntity, TFilterDto>();
        var combinedSpec = _specificationCombiner.Combine(spec, predicate);
        var query = ApplySpecification(combinedSpec);

        return await query.AsNoTracking().ToListAsync();
    }

    public virtual TEntity Update(TEntity newEntity)
    {
        _context.Set<TEntity>().Update(newEntity);

        return newEntity;
    }

    public virtual void UpdateRange(IEnumerable<TEntity> newEntities)
    {
        _context.Set<TEntity>().UpdateRange(newEntities);
    }

    public virtual TEntity Delete(TPrimaryKey id)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        TEntity entity;

        if (id is ITuple)
        {
            var predicate =
                CompositeKeyHelper.BuildCompositeKeyPredicate<TEntity, TPrimaryKey>(id);

            entity = query.FirstOrDefault(predicate)
                ?? throw new ArgumentException($"Entity with id {id} not found.");
        }

        else
        {
            entity = query.FirstOrDefault(e =>
                EF.Property<TPrimaryKey>(e, "Id")!.Equals(id))
                ?? throw new ArgumentException($"Entity with id {id} not found.");
        }

        _context.Set<TEntity>().Remove(entity);

        return entity;
    }

    public virtual void DeleteRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
    }

    public virtual async Task<long> GetCountAsync(IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        return await query.CountAsync();
    }

    public virtual async Task<decimal> GetSumAsync(
        Expression<Func<TEntity, decimal>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        return await query.SumAsync(selector);
    }

    public virtual async Task<decimal> GetAverageAsync(
        Expression<Func<TEntity, decimal>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        return await query.AverageAsync(selector);
    }

    public virtual async Task<TResult> GetMaxAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        return await query.MaxAsync(selector);
    }

    public virtual async Task<TResult> GetMinAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        IBaseSpecification<TEntity>? spec = null)
    {
        var query = ApplySpecification(spec);

        return await query.MinAsync(selector);
    }

    private IQueryable<TEntity> ApplySpecification(IBaseSpecification<TEntity>? spec)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (spec == null)
            return query;

        return query
            .ApplyCriteria(spec.Criteria)
            .ApplyIncludes(spec.Includes)
            .ApplyIncludeChains(spec.IncludeChains)
            .ApplyOrderBy(spec.OrderBy)
            .ApplyOrderByDescending(spec.OrderByDescending);
    }
}
