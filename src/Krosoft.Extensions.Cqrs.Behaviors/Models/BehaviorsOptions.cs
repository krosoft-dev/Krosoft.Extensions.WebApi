using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Cqrs.Behaviors.Models;

public sealed class BehaviorsOptions
{
    public IServiceCollection Services { get; }

    public BehaviorsOptions(IServiceCollection services)
    {
        Services = services;
    }

}