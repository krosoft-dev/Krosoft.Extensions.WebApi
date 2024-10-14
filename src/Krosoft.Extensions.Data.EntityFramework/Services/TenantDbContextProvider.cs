using Krosoft.Extensions.Data.EntityFramework.Interfaces;

namespace Krosoft.Extensions.Data.EntityFramework.Services;

public class TenantDbContextProvider : ITenantDbContextProvider
{
    private readonly string _tenantId;

    public TenantDbContextProvider()
    {
        _tenantId = "";
    }

    public TenantDbContextProvider(string tenantId)
    {
        _tenantId = tenantId;
    }

    public string GetTenantId() => _tenantId;
}