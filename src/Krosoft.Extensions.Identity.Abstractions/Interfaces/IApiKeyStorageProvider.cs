namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IApiKeyStorageProvider
{
    Task<string?> GetIdentifiantAsync(string? key, CancellationToken cancellationToken);
    Task<string?> GetKeyAsync(string? identifiant, CancellationToken cancellationToken);
}