using Krosoft.Extensions.Cqrs.Behaviors.Extensions;
using Krosoft.Extensions.Cqrs.Behaviors.Identity.PipelineBehaviors;
using Krosoft.Extensions.Cqrs.Behaviors.Models;

namespace Krosoft.Extensions.Cqrs.Behaviors.Identity.Extensions;

public static class BehaviorsOptionsExtensions
{
    public static BehaviorsOptions AddIdentity(this BehaviorsOptions options)
    {
        options.Add(typeof(AuthorizationPipelineBehavior<,>));

        return options;
    }

    public static BehaviorsOptions AddApiKey(this BehaviorsOptions options)
    {
        options.Add(typeof(ApiKeyPipelineBehavior<,>));

        return options;
    }
}