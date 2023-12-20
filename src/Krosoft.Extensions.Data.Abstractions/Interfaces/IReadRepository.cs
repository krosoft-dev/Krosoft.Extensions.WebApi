namespace Krosoft.Extensions.Data.Abstractions.Interfaces;

public interface IReadRepository<out TEntity> : IDisposable where TEntity : class
{
    IQueryable<TEntity> Query();
}