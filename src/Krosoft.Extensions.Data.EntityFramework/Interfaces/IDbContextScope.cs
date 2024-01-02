using Krosoft.Extensions.Data.Abstractions.Interfaces;

namespace Krosoft.Extensions.Data.EntityFramework.Interfaces;

public interface IDbContextScope : IReadDbContextScope
{
    public IUnitOfWork GetUnitOfWork();

    public IWriteRepository<TEntity> GetWriteRepository<TEntity>()
        where TEntity : class;
}