namespace Todo.Domain.Shared;

public interface IUnitOfWork
{
    Task SaveEntitiesAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}