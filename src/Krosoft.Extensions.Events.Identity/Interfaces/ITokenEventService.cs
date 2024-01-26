using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Events.Interfaces;
using MediatR;

namespace Krosoft.Extensions.Events.Identity.Interfaces;

public interface ITokenEventService : IEventService
{
    void Publish(Func<KrosoftToken, INotification> func, CancellationToken cancellationToken);
}