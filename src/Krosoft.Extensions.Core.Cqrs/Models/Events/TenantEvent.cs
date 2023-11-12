using MediatR;

namespace Krosoft.Extensions.Core.Cqrs.Models.Events;

public abstract class TenantEvent : INotification
{
    protected TenantEvent(string tenantId)
    {
        TenantId = tenantId;
    }

    public string TenantId { get; }
}