using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Krosoft.Extensions.Blocking.Abstractions.Models.Enums;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Blocking.Services;

public abstract class BlockingService
{
    private const string Blocked = "blocked";
    private readonly IBlockingStorageProvider _blockingStorageProvider;
    private readonly BlockType _blockType;
    private readonly ILogger<BlockingService> _logger;

    protected BlockingService(BlockType blockType,
                              IBlockingStorageProvider blockingStorageProvider,
                              ILogger<BlockingService> logger)
    {
        _blockType = blockType;
        _blockingStorageProvider = blockingStorageProvider;
        _logger = logger;
    }

    public async Task BlockAsync(ISet<string> keys,
                                 CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Blocking {_blockType} : {string.Join(",", keys)}");

        var entries = new Dictionary<string, string>();
        foreach (var key in keys)
        {
            entries.Add(key, Blocked);
        }

        var collectionKey = GetCollectionKey();
        await _blockingStorageProvider.SetAsync(collectionKey, entries, cancellationToken);
    }

    public async Task BlockAsync(string key,
                                 CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Blocking {_blockType} : {key}");
        var collectionKey = GetCollectionKey();
        await _blockingStorageProvider.SetAsync(collectionKey, key, Blocked, cancellationToken);
    }

    public async Task<IEnumerable<string>> GetBlockedAsync(CancellationToken cancellationToken)
    {
        var collectionKey = GetCollectionKey();
        var keys = await _blockingStorageProvider.GetKeysAsync(collectionKey, cancellationToken);
        return keys;
    }

    private string GetCollectionKey() => $"Blocking_{_blockType.ToString()}";

    public async Task<bool> IsBlockedAsync(string key,
                                           CancellationToken cancellationToken)
    {
        var collectionKey = GetCollectionKey();
        var isExist = await _blockingStorageProvider.IsSetAsync(collectionKey, key, cancellationToken);
        if (isExist)
        {
            _logger.LogDebug($"{_blockType} is blocked : {key}");
        }

        return isExist;
    }

    public async Task<long> UnblockAsync(ISet<string> keys,
                                         CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Unblocking {_blockType} : {string.Join(",", keys)}");
        var collectionKey = GetCollectionKey();
        var number = await _blockingStorageProvider.RemoveAsync(collectionKey, keys, cancellationToken);
        return number;
    }

    public async Task<bool> UnblockAsync(string key,
                                         CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Unblocking {_blockType} : {key}");

        var collectionKey = GetCollectionKey();
        var isDelete = await _blockingStorageProvider.RemoveAsync(collectionKey, key, cancellationToken);
        return isDelete;
    }
}