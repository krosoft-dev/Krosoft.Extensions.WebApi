using Krosoft.Extensions.Data.EntityFramework.Contexts;

namespace Krosoft.Extensions.Data.EntityFramework.Models;

public class TenantAuditableDbContextSettings<T> : ITenantAuditableDbContextSettings<T>
    where T : KrosoftTenantAuditableContext
{
    public TenantAuditableDbContextSettings(string tenantId, DateTime now, string utilisateurId)
    {
        Now = now;
        UtilisateurId = utilisateurId;
        TenantId = tenantId;
    }

    public DateTime Now { get; }

    public string TenantId { get; }

    public string UtilisateurId { get; }
}