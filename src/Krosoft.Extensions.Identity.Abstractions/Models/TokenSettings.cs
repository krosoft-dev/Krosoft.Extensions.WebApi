namespace Krosoft.Extensions.Identity.Abstractions.Models;

public record TokenSettings
{
    /// <summary>
    /// Durée de vie du jeton (en jours).
    /// </summary>
    public double TokenLifespan { get; set; }
}