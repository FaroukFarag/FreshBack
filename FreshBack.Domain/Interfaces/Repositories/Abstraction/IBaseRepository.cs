namespace FreshBack.Domain.Interfaces.Repositories.Abstraction;

public interface IBaseRepository<TEntity, TPrimaryKey> :
    IReadRepository<TEntity, TPrimaryKey>,
    IWriteRepository<TEntity, TPrimaryKey>,
    IAggregateRepository<TEntity>
    where TEntity : class
{
}