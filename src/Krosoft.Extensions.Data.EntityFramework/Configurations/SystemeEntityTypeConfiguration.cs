using Krosoft.Extensions.Data.Abstractions.Models;

namespace Krosoft.Extensions.Data.EntityFramework.Configurations;

public abstract class SystemeEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : Entity
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="SystemeEntityTypeConfiguration{T}" />.
    /// </summary>
    protected SystemeEntityTypeConfiguration(string tableName) : base("Systeme", tableName)
    {
    }
}