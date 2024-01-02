namespace Krosoft.Extensions.Data.Abstractions.Interfaces;

public interface IGenericWriteRepository
{
    void Delete<TEntity>(TEntity entity) where TEntity : class;    
    IQueryable<TEntity> Query<TEntity>() where TEntity : class;    
    ValueTask<TEntity?> GetAsync<TEntity>(params object[] key) where TEntity : class;
    void Insert<TEntity>(TEntity entity) where TEntity : class;
    void Update<TEntity>(TEntity entity) where TEntity : class;
}