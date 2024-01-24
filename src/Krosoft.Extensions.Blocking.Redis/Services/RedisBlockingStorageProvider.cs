using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Krosoft.Extensions.Cache.Distributed.Redis.Interfaces;

namespace Krosoft.Extensions.Blocking.Redis.Services;

public class RedisBlockingStorageProvider : IBlockingStorageProvider
{
    private readonly IDistributedCacheProvider _distributedCacheProvider;

    public RedisBlockingStorageProvider(IDistributedCacheProvider distributedCacheProvider)
    {
        _distributedCacheProvider = distributedCacheProvider;
    }

    public async Task<bool> IsSetAsync(string collectionKey,
                                       string key,
                                       CancellationToken cancellationToken)
    {
        var isExist = await _distributedCacheProvider.IsExistRowAsync(collectionKey, key, cancellationToken);

        return isExist;
    }

    public async Task<long> RemoveAsync(string collectionKey,
                                        ISet<string> keys,
                                        CancellationToken cancellationToken)
    {
        var number = await _distributedCacheProvider.DeleteRowsAsync(collectionKey, keys, cancellationToken);
        return number;
    }

    public async Task<bool> RemoveAsync(string collectionKey,
                                        string key,
                                        CancellationToken cancellationToken)
    {
        var isDelete = await _distributedCacheProvider.DeleteRowAsync(collectionKey, key, cancellationToken);
        return isDelete;
    }

    public async Task SetAsync(string collectionKey,
                               string key,
                               string entry,
                               CancellationToken cancellationToken)
    {
        await _distributedCacheProvider.SetRowAsync(collectionKey, key, entry, cancellationToken);
    }

    public async Task SetAsync(string collectionKey,
                               IDictionary<string, string> entryByKey,
                               CancellationToken cancellationToken)
    {
        await _distributedCacheProvider.SetRowAsync(collectionKey, entryByKey, cancellationToken);
    }

    public async Task<IEnumerable<string>> GetKeysAsync(string collectionKey,
                                                        CancellationToken cancellationToken) =>
        await _distributedCacheProvider.ReadRowsAsync<string>(collectionKey, cancellationToken);
}