using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public UnitOfWork(DbContext dbContext)
    {
        _context = dbContext;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public int SaveChanges()
        => _context.SaveChanges();

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => await _context.SaveChangesAsync(cancellationToken);
}