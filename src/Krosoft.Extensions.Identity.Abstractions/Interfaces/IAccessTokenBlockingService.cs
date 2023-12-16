namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IAccessTokenBlockingService
{
    Task BlockAsync(string accessToken, CancellationToken cancellationToken);
    Task BlockAsync(ISet<string> accessTokens, CancellationToken cancellationToken);
    Task BlockAsync(CancellationToken cancellationToken);
    Task<bool> IsBlockedAsync(CancellationToken cancellationToken);
    Task<bool> UnblockAsync(string accessToken, CancellationToken cancellationToken);
    Task<long> UnblockAsync(ISet<string> accessTokens, CancellationToken cancellationToken);
}