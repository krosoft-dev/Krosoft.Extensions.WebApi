using Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Queries;
using Krosoft.Extensions.Cache.Distributed.Redis.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Handlers.Queries;

public class CacheQueryHandler : IRequestHandler<CacheQuery, IDictionary<string, long>>
{
    private readonly IDistributedCacheProvider _distributedCacheProvider;
    private readonly ILogger<CacheQueryHandler> _logger;

    public CacheQueryHandler(ILogger<CacheQueryHandler> logger, IDistributedCacheProvider distributedCacheProvider)
    {
        _distributedCacheProvider = distributedCacheProvider;
        _logger = logger;
    }

    public async Task<IDictionary<string, long>> Handle(CacheQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération du contenu du cache...");

        var lengthByKey = new Dictionary<string, long>();
        var keys = _distributedCacheProvider.GetKeys(string.Empty);
        foreach (var key in keys)
        {
            var length = await _distributedCacheProvider.GetLengthAsync(key, cancellationToken);
            lengthByKey.Add(key, length);
        }

        return lengthByKey;
    }
}