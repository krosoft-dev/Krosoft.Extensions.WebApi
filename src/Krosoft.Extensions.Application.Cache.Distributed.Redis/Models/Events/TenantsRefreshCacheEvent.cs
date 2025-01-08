using Krosoft.Extensions.Cqrs.Models.Events;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Events;

public record TenantsRefreshCacheEvent : TenantsEvent
{
    public TenantsRefreshCacheEvent(ISet<string> tenantsId) : base(tenantsId)
    {
    }
}