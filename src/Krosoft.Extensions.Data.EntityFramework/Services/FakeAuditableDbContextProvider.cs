using Krosoft.Extensions.Data.EntityFramework.Interfaces;

namespace Krosoft.Extensions.Data.EntityFramework.Services;

public class FakeAuditableDbContextProvider : IAuditableDbContextProvider
{
    public DateTime GetNow() => new DateTime(2012, 9, 28);

    public string GetUtilisateurId() => "Fake_Utilisateur_Id";
}