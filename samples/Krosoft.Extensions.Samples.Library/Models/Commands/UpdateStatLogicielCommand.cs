using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.Library.Models.Commands;

public record UpdateStatLogicielCommand : PayloadBaseCommand
{
    public UpdateStatLogicielCommand(string payload) : base(payload)
    {
    }
}