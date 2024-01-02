using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Repositories;

public class GenericReadRepository : IGenericReadRepository
{
    private readonly DbContext _dbContext;

    public GenericReadRepository(DbContext dbDbContext)
    {
        Guard.IsNotNull(nameof(dbDbContext), dbDbContext);

        _dbContext = dbDbContext;
    }

    public IQueryable<TEntity> Query<TEntity>() where TEntity : class
    {
        var dbSet = _dbContext.Set<TEntity>();
        return dbSet.AsNoTracking();
    }

    public async Task<IEnumerable<TEntity>> ToListAsync<TEntity>(IQueryable<TEntity> query,
                                                                 CancellationToken cancellationToken)
        where TEntity : class
        => await query.ToListAsync(cancellationToken);
}