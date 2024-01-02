namespace Krosoft.Extensions.Data.EntityFramework.Interfaces;

public interface ITenantDbContextProvider
{
    string GetTenantId();
}