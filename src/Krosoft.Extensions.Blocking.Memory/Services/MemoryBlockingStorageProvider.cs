using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Krosoft.Extensions.Cache.Memory.Interfaces;

namespace Krosoft.Extensions.Blocking.Memory.Services;

public class MemoryBlockingStorageProvider : IBlockingStorageProvider
{
    private readonly ICacheProvider _cacheProvider;

    public MemoryBlockingStorageProvider(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    public Task<IEnumerable<string>> GetKeysAsync(string collectionKey,
                                                  CancellationToken cancellationToken)
    {
        var keys = _cacheProvider.GetKeys().Where(x => x.StartsWith(collectionKey)).Select(x => x.Replace($"{collectionKey}_", ""));
        return Task.FromResult(keys);
    }

    public Task<bool> IsSetAsync(string collectionKey,
                                 string key,
                                 CancellationToken cancellationToken)
    {
        var isExist = _cacheProvider.IsSet(GetFullKey(collectionKey, key));
        return Task.FromResult(isExist);
    }

    public async Task SetAsync(string collectionKey,
                               IDictionary<string, string> entryByKey,
                               CancellationToken cancellationToken)
    {
        foreach (var keyValuePair in entryByKey)
        {
            await SetAsync(collectionKey, keyValuePair.Key, keyValuePair.Value, cancellationToken);
        }
    }

    public Task<bool> RemoveAsync(string collectionKey,
                                  string key,
                                  CancellationToken cancellationToken)
    {
        _cacheProvider.Remove(GetFullKey(collectionKey, key));

        return Task.FromResult(true);
    }

    public Task SetAsync(string collectionKey,
                         string key,
                         string entry,
                         CancellationToken cancellationToken)
    {
        _cacheProvider.Set(GetFullKey(collectionKey, key), entry);

        return Task.CompletedTask;
    }

    public async Task<long> RemoveAsync(string collectionKey,
                                        ISet<string> keys,
                                        CancellationToken cancellationToken)
    {
        long number = 0;
        foreach (var key in keys)
        {
            await RemoveAsync(collectionKey, key, cancellationToken);
            number++;
        }

        return number;
    }

    private static string GetFullKey(string collectionKey, string key) => $"{collectionKey}_{key}";
}