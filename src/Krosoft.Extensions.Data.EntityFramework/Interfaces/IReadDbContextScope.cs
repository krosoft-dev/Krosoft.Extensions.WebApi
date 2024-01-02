using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.Interfaces;

public interface IReadDbContextScope : IServiceScope
{
    public IReadRepository<TEntity> GetReadRepository<TEntity>()
        where TEntity : class;
}