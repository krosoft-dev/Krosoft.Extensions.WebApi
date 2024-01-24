namespace Krosoft.Extensions.Blocking.Abstractions.Interfaces;

public interface IBlockingStorageProvider
{
    Task<IEnumerable<string>> GetKeysAsync(string collectionKey,
                                           CancellationToken cancellationToken);

    Task<bool> IsSetAsync(string collectionKey, string key, CancellationToken cancellationToken);
    Task<bool> RemoveAsync(string collectionKey, string key, CancellationToken cancellationToken);
    Task<long> RemoveAsync(string collectionKey, ISet<string> keys, CancellationToken cancellationToken);
    Task SetAsync(string collectionKey, string key, string entry, CancellationToken cancellationToken);
    Task SetAsync(string collectionKey, IDictionary<string, string> entryByKey, CancellationToken cancellationToken);
}