using MediatR;

namespace Krosoft.Extensions.Core.Cqrs.Models.Commands;

public abstract class PayloadBaseCommand : BaseCommand<Unit>
{
    protected PayloadBaseCommand(string payload)
    {
        Payload = payload;
    }

    public string Payload { get; }
}