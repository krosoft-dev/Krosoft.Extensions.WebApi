using Krosoft.Extensions.Data.Abstractions.Models;

namespace Krosoft.Extensions.Data.EntityFramework.Configurations;

public abstract class ReferentielEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : Entity
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="ReferentielEntityTypeConfiguration{T}" />.
    /// </summary>
    protected ReferentielEntityTypeConfiguration(string tableName) : base("Referentiel", tableName)
    {
    }
}