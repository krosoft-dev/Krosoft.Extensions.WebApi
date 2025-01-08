using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Tools;
using MediatR;

namespace Krosoft.Extensions.Cqrs.Models.Events;

public abstract record KrosoftTokenBaseEvent : INotification
{
    protected KrosoftTokenBaseEvent(KrosoftToken krosoftToken)
    {
        Guard.IsNotNull(nameof(krosoftToken), krosoftToken);

        KrosoftToken = krosoftToken;
    }

    public KrosoftToken KrosoftToken { get; }
}