using Krosoft.Extensions.Cqrs.Behaviors.Models;
using Krosoft.Extensions.Cqrs.Behaviors.PipelineBehaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Cqrs.Behaviors.Extensions;

public static class BehaviorsOptionsExtensions
{
    public static BehaviorsOptions Add(this BehaviorsOptions options, Type type)
    {
        options.Services.AddTransient(typeof(IPipelineBehavior<,>), type);
        return options;
    }

    public static BehaviorsOptions AddLogging(this BehaviorsOptions options)
    {
        options.Add(typeof(LoggingPipelineBehavior<,>));
        return options;
    }
}