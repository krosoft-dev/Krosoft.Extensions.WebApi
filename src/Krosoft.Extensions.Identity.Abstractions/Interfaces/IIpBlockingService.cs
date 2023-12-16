namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IIpBlockingService
{
    Task BlockAsync(string remoteIp, CancellationToken cancellationToken);
    Task BlockAsync(ISet<string> remotesIp, CancellationToken cancellationToken);
    Task<bool> IsBlockedAsync(string remoteIp, CancellationToken cancellationToken);
    Task<bool> UnblockAsync(string remoteIp, CancellationToken cancellationToken);
    Task<long> UnblockAsync(ISet<string> remotesIp, CancellationToken cancellationToken);
}