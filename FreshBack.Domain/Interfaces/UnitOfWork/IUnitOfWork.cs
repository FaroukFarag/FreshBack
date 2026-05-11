using Microsoft.EntityFrameworkCore.Storage;

namespace FreshBack.Domain.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<bool> Complete();
}