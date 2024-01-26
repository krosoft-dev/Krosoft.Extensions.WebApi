using Krosoft.Extensions.Events.Interfaces;
using Krosoft.Extensions.Events.Services;
using Krosoft.Extensions.Jobs.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Events.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.AddFireForget();
        services.AddTransient<IEventService, EventService>();

        return services;
    }
}