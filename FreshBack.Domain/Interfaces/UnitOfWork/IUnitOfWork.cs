namespace FreshBack.Domain.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    Task<bool> Complete();
}