using Krosoft.Extensions.Data.Abstractions.Models;

namespace Krosoft.Extensions.Data.EntityFramework.Configurations;

/// <summary>
/// Configuration de base pour le mapping des entités <see cref="Entity" />.
/// </summary>
/// <typeparam name="T">Type de l'entité.</typeparam>
public abstract class ActiviteEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : Entity
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="ActiviteEntityTypeConfiguration{T}" />.
    /// </summary>
    protected ActiviteEntityTypeConfiguration(string tableName) : base("Activite", tableName)
    {
    }
}