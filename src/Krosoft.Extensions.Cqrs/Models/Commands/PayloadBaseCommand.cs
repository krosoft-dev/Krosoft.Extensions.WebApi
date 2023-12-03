namespace Krosoft.Extensions.Cqrs.Models.Commands;

public abstract class PayloadBaseCommand : BaseCommand
{
    protected PayloadBaseCommand(string payload)
    {
        Payload = payload;
    }

    public string Payload { get; }
}