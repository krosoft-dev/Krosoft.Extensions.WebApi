using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Hosting.Models;
using Krosoft.Extensions.Hosting.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Hosting.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScheduledJob<T, TConfig>(this IServiceCollection services,
                                                                 Action<TConfig> options)
        where T : ScheduledHostedService
        where TConfig : ScheduleConfig, new()
    {
        Guard.IsNotNull(nameof(options), options);

        var config = new TConfig();
        options.Invoke(config);

        services.AddSingleton(config);
        services.AddHostedService<T>();
        return services;
    }

    public static IServiceCollection AddScheduledJob<T>(this IServiceCollection services,
                                                        Action<ScheduleConfig> options)
        where T : ScheduledHostedService
    {
        services.AddScheduledJob<T, ScheduleConfig>(options);
        return services;
    }

    public static IServiceCollection AddScheduledJob<T>(this IServiceCollection services,
                                                        IConfiguration configuration) where T : ScheduledHostedService
    {
        services.AddScheduledJob<T>(c => { c.Interval = configuration.GetValue<TimeSpan>("AppSettings:CacheRefreshTimeSpan"); });

        return services;
    }
}