using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Models.Commands;

public class LogicielsDeleteCommand : AuthBaseCommand
{
    public LogicielsDeleteCommand()
    {
        Ids = new HashSet<Guid>();
    }

    public IReadOnlySet<Guid> Ids { get; set; }
}