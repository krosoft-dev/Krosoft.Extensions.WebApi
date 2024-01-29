using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Cqrs.Behaviors.Models;

public sealed class BehaviorsOptions
{
    public BehaviorsOptions(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }
}