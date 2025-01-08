using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Cqrs.Models.Events;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Events;

public record KrosoftTokenRefreshCacheEvent : KrosoftTokenBaseEvent
{
    public KrosoftTokenRefreshCacheEvent(KrosoftToken positiveToken) : base(positiveToken)
    {
    }
}