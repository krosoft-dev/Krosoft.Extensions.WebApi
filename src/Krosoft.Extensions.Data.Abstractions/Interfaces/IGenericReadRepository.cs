namespace Krosoft.Extensions.Data.Abstractions.Interfaces;

public interface IGenericReadRepository
{
    IQueryable<TEntity> Query<TEntity>() where TEntity : class;

    Task<IEnumerable<TEntity>> ToListAsync<TEntity>(IQueryable<TEntity> query,
                                                    CancellationToken cancellationToken) where TEntity : class;
}