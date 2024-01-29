using Krosoft.Extensions.Cqrs.Behaviors.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Cqrs.Behaviors.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBehaviors(this IServiceCollection services,
                                                  Action<BehaviorsOptions>? configure = null)
    {
        var options = new BehaviorsOptions(services);
        configure?.Invoke(options);

        return services;
    }
}