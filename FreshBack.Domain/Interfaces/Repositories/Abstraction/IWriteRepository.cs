namespace FreshBack.Domain.Interfaces.Repositories.Abstraction;

public interface IWriteRepository<TEntity, TPrimaryKey> where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity);

    Task CreateRangeAsync(IEnumerable<TEntity> entities);

    TEntity Update(TEntity newEntity);

    void UpdateRange(IEnumerable<TEntity> newEntities);

    TEntity Delete(TPrimaryKey id);

    void DeleteRange(IEnumerable<TEntity> entities);
}
