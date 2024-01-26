using Krosoft.Extensions.Events.Extensions;
using Krosoft.Extensions.Events.Identity.Interfaces;
using Krosoft.Extensions.Events.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Events.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTokenEvents(this IServiceCollection services)
    {
        services.AddEvents();
        services.AddTransient<ITokenEventService, TokenEventService>();

        return services;
    }
}