using Krosoft.Extensions.Data.EntityFramework.Contexts;

namespace Krosoft.Extensions.Data.EntityFramework.Models;

public class TenantDbContextSettings<T> : ITenantDbContextSettings<T> where T : KrosoftTenantContext
{
    public TenantDbContextSettings(string tenantId)
    {
        TenantId = tenantId;
    }

    public string TenantId { get; }
}