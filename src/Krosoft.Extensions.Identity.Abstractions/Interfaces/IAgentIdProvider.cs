namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IAgentIdProvider
{
    Task<string?> GetAgentIdAsync(CancellationToken cancellationToken);
}