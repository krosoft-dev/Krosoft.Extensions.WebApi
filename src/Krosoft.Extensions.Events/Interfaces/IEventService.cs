using MediatR;

namespace Krosoft.Extensions.Events.Interfaces;

public interface IEventService
{
    void Publish(INotification notification, CancellationToken cancellationToken);
}