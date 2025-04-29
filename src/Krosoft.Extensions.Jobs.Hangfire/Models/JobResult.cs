namespace Krosoft.Extensions.Jobs.Hangfire.Models;

/// <summary>
/// Représente le résultat d'un job.
/// </summary>
public class JobResult
{
    public JobResult(string? message,
                     TimeSpan elapsed,
                     Exception? exception)
    {
        Message = message;
        Elapsed = elapsed;
        Exception = exception;
    }

    /// <summary>
    /// Message renvoyé à l'utilisateur.
    /// </summary>
    public string? Message { get; }

    public TimeSpan Elapsed { get; }

    public Exception? Exception { get; }

    public bool IsError => Exception != null;
}