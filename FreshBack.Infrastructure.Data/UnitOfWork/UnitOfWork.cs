using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace FreshBack.Infrastructure.Data.UnitOfWork;

public class UnitOfWork(FreshBackDbContext context) : IUnitOfWork
{
    private readonly FreshBackDbContext _context = context;

    public async Task<IDbContextTransaction> BeginTransactionAsync()
        => await _context.Database.BeginTransactionAsync();

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}
