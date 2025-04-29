namespace Krosoft.Extensions.Jobs.Hangfire.Models;

public interface IRecurringJob
{
    string Type { get; }

    Task ExecuteAsync(string identifiant);
}