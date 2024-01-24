namespace Krosoft.Extensions.Blocking.Abstractions.Interfaces;

public interface IBlockingService
{
    Task BlockAsync(string key, CancellationToken cancellationToken);
    Task BlockAsync(ISet<string> keys, CancellationToken cancellationToken);
    Task<IEnumerable<string>> GetBlockedAsync(CancellationToken cancellationToken);
    Task<bool> IsBlockedAsync(string key, CancellationToken cancellationToken);
    Task<bool> UnblockAsync(string key, CancellationToken cancellationToken);
    Task<long> UnblockAsync(ISet<string> keys, CancellationToken cancellationToken);
}