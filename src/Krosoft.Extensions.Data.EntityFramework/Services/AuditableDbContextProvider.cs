using Krosoft.Extensions.Data.EntityFramework.Interfaces;

namespace Krosoft.Extensions.Data.EntityFramework.Services;

public class AuditableDbContextProvider : IAuditableDbContextProvider
{
    private readonly DateTime _now;
    private readonly string _utilisateurId;

    public AuditableDbContextProvider()
    {
        _now = DateTime.MinValue;
        _utilisateurId = string.Empty;
    }

    public AuditableDbContextProvider(DateTime now, string utilisateurId)
    {
        _now = now;
        _utilisateurId = utilisateurId;
    }

    public DateTime GetNow() => _now;

    public string GetUtilisateurId() => _utilisateurId;
}