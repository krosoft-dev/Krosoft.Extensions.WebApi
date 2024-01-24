namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IAccessTokenProvider
{
    Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken);
}