namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IIdentifierProvider
{
    Task<string?> GetIdentifierAsync(CancellationToken cancellationToken);
}