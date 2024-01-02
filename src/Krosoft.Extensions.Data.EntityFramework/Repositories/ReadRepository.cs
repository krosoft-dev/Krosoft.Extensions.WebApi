using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Repositories;

public sealed class ReadRepository<TEntity> : IReadRepository<TEntity>
    where TEntity : class

{
    private readonly DbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public ReadRepository(DbContext dbDbContext)
    {
        Guard.IsNotNull(nameof(dbDbContext), dbDbContext);

        _dbContext = dbDbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public IQueryable<TEntity> Query() => _dbSet.AsNoTracking();
}