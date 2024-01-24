using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Krosoft.Extensions.Blocking.Redis.Services;
using Krosoft.Extensions.Cache.Distributed.Redis.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Blocking.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedisBlockingStorage(this IServiceCollection services)
    {
        services.AddDistributedCacheExt();
        services.AddTransient<IBlockingStorageProvider, RedisBlockingStorageProvider>();

        return services;
    }
}