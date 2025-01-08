using Krosoft.Extensions.Cqrs.Models.Events;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Events;

public record TenantRefreshCacheEvent : TenantEvent
{
    public TenantRefreshCacheEvent(string tenantId) : base(tenantId)
    {
    }
}