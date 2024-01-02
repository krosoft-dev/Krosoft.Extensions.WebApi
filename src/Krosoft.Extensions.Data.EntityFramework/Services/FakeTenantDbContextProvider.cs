using Krosoft.Extensions.Data.EntityFramework.Interfaces;

namespace Krosoft.Extensions.Data.EntityFramework.Services;

public class FakeTenantDbContextProvider : ITenantDbContextProvider
{
    public string GetTenantId() => "Fake_Tenant_Id";
}