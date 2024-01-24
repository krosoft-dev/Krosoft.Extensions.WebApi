using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Krosoft.Extensions.Blocking.Memory.Services;
using Krosoft.Extensions.Cache.Memory.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Blocking.Memory.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMemoryBlockingStorage(this IServiceCollection services)
    {
        services.AddMemoryCacheExt();
        services.AddTransient<IBlockingStorageProvider, MemoryBlockingStorageProvider>();

        return services;
    }
}