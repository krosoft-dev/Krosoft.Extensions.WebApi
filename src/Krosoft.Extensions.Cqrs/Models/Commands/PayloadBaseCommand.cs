namespace Krosoft.Extensions.Cqrs.Models.Commands;

public abstract record PayloadBaseCommand : BaseCommand
{
    protected PayloadBaseCommand(string payload)
    {
        Payload = payload;
    }

    public string Payload { get; }
}