namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IApiKeyProvider
{
    Task<string?> GetApiKeyAsync(CancellationToken cancellationToken);
}