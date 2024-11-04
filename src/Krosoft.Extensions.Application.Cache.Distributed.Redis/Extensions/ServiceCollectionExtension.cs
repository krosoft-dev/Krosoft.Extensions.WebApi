using Krosoft.Extensions.Application.Cache.Distributed.Redis.Models;
using Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Commands;
using Krosoft.Extensions.Application.Cache.Distributed.Redis.Services;
using Krosoft.Extensions.Hosting.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCacheHandlers(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtension).Assembly));

        return services;
    }

    public static IServiceCollection AddCacheRefreshHostedService(this IServiceCollection services,
                                                                  IConfiguration configuration)
    {
        services.AddCacheRefreshHostedService(new AuthCacheRefreshCommand(false, false),
                                              configuration);

        return services;
    }

    public static IServiceCollection AddCacheRefreshHostedService<TCommand>(this IServiceCollection services,
                                                                            TCommand command,
                                                                            IConfiguration configuration)
    {
        services.AddScheduledJob<CacheRefreshHostedService<TCommand>, CacheScheduleConfig<TCommand>>(c =>
        {
            c.Command = command;
            c.Interval = configuration.GetValue<TimeSpan>("AppSettings:CacheRefreshTimeSpan");
        });

        return services;
    }
}