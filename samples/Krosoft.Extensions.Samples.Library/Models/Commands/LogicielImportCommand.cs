using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.Library.Models.Commands;

public record LogicielImportCommand : AuthBaseCommand<int>
{
    public LogicielImportCommand(IEnumerable<string> files)
    {
        Files = files;
    }

    public IEnumerable<string> Files { get; }
}