using Krosoft.Extensions.Data.EntityFramework.Contexts;

namespace Krosoft.Extensions.Data.EntityFramework.Models;

public record AuditableDbContextSettings<T> : IAuditableDbContextSettings<T> where T : KrosoftAuditableContext
{
    public AuditableDbContextSettings(DateTime now, string utilisateurId)
    {
        Now = now;
        UtilisateurId = utilisateurId;
    }

    public DateTime Now { get; }

    public string UtilisateurId { get; }
}