using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Infrastructure.Data.Context;

namespace FreshBack.Infrastructure.Data.UnitOfWork;

public class UnitOfWork(FreshBackDbContext context) : IUnitOfWork
{
    private readonly FreshBackDbContext _context = context;

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}
