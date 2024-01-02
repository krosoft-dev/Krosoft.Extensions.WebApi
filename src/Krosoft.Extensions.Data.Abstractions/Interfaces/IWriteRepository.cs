using System.Linq.Expressions;
using Krosoft.Extensions.Data.Abstractions.Models;

namespace Krosoft.Extensions.Data.Abstractions.Interfaces;

public interface IWriteRepository<TEntity> : IDisposable where TEntity : class
{
    TEntity? Get(params object[] key);

    ValueTask<TEntity?> GetAsync(params object[] key);
    IQueryable<TEntity> Query();

    void Insert(TEntity entity);

    void InsertRange(IEnumerable<TEntity> entities);

    void Update(TEntity entityToUpdate);

    void Update(TEntity entityToUpdate, params Expression<Func<TEntity, object>>[] propertiesExpression);

    void UpdateRange(IEnumerable<TEntity> entities);

    void UpdateRange(IEnumerable<TEntity> entities, params Expression<Func<TEntity, object>>[] propertiesExpression);

    void Delete(TEntity entity);

    void DeleteById(params object[] key);

    Task DeleteByIdAsync(params object[] key);

    void DeleteRange(IEnumerable<TEntity> entities);

    void DeleteRange(Expression<Func<TEntity, bool>> predicate);

    void DeleteRange();

    void InsertUpdateDelete(CrudBusiness<TEntity> crudBusiness);
}