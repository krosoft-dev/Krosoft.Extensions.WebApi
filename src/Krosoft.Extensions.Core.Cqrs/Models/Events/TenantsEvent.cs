using MediatR;

namespace Krosoft.Extensions.Core.Cqrs.Models.Events;

public abstract class TenantsEvent : INotification
{
    protected TenantsEvent(ISet<string> tenantsId)
    {
        TenantsId = tenantsId;
    }

    public ISet<string> TenantsId { get; }
}