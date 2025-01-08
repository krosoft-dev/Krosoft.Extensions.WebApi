using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.Library.Models.Commands;

public record LogicielsDeleteCommand : AuthBaseCommand
{
    public LogicielsDeleteCommand()
    {
        Ids = new HashSet<Guid>();
    }

    public IReadOnlySet<Guid> Ids { get; set; }
}