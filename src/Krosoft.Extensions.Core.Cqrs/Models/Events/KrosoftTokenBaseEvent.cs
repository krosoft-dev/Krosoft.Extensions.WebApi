using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Tools;
using MediatR;

namespace Krosoft.Extensions.Core.Cqrs.Models.Events;

public abstract class KrosoftTokenBaseEvent : INotification
{
    protected KrosoftTokenBaseEvent(KrosoftToken krosoftToken)
    {
        Guard.IsNotNull(nameof(krosoftToken), krosoftToken);

        KrosoftToken = krosoftToken;
    }

    public KrosoftToken KrosoftToken { get; }
}