using MediatR;

namespace Krosoft.Extensions.Cqrs.Models.Events;

public abstract record TenantsEvent : INotification
{
    protected TenantsEvent(ISet<string> tenantsId)
    {
        TenantsId = tenantsId;
    }

    public ISet<string> TenantsId { get; }
}