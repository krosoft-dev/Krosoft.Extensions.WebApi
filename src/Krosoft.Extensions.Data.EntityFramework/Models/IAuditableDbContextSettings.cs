using Krosoft.Extensions.Data.EntityFramework.Contexts;

namespace Krosoft.Extensions.Data.EntityFramework.Models;

public interface IAuditableDbContextSettings<T> : IDbContextSettings<T> where T : KrosoftContext
{
    string UtilisateurId { get; }
    DateTime Now { get; }
}