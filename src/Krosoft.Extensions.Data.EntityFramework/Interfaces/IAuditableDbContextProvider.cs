namespace Krosoft.Extensions.Data.EntityFramework.Interfaces;

public interface IAuditableDbContextProvider
{
    DateTime GetNow();
    string GetUtilisateurId();
}