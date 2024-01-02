using Krosoft.Extensions.Data.EntityFramework.Contexts;

namespace Krosoft.Extensions.Data.EntityFramework.Models;

public interface ITenantDbContextSettings<T> : IDbContextSettings<T> where T : KrosoftContext
{
    string TenantId { get; }
}