namespace Krosoft.Extensions.Data.Abstractions.Interfaces;

public interface IUnitOfWork : IDisposable
{
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}