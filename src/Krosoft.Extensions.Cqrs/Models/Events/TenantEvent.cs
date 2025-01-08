using MediatR;

namespace Krosoft.Extensions.Cqrs.Models.Events;

public abstract record TenantEvent : INotification
{
    protected TenantEvent(string tenantId)
    {
        TenantId = tenantId;
    }

    public string TenantId { get; }
}