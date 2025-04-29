namespace Krosoft.Extensions.Jobs.Hangfire.Models;

/// <summary>
/// Contexte d'un job.
/// </summary>
public record JobContext
{
    public Guid Id { get; set; }

    /// <summary>
    /// Identifiant du job.
    /// </summary>
    public string? Cle { get; set; }

    /// <summary>
    /// Type de job.
    /// </summary>
    public string? TypeCode { get; set; }

    public string? ParametresJson { get; set; }
    public string? QueueName { get; set; }
}