//

//using Krosoft.Extensions.Identity.Services;
//using Krosoft.Extensions.Infrastructure.Services;
//using Krosoft.Extensions.Jobs.Extensions;
//using Swashbuckle.AspNetCore.SwaggerGen;

//namespace IzRoadbook.Extensions.Extensions;

using Krosoft.Extensions.Cqrs.Behaviors.Extensions;
using Krosoft.Extensions.Cqrs.Behaviors.Models;
using Krosoft.Extensions.Cqrs.Behaviors.Validations.PipelineBehaviors;

namespace Krosoft.Extensions.Cqrs.Behaviors.Validations.Extensions;

public static class BehaviorsOptionsExtensions
{
    public static BehaviorsOptions AddValidations(this BehaviorsOptions options)
    {
        options.Add(  typeof(ValidatorPipelineBehavior<,>));

        return options;
    }
}