using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Models.Commands;

public class LogicielImportCommand : AuthBaseCommand<int>
{
    public LogicielImportCommand(IEnumerable<string> files)
    {
        Files = files;
    }

    public IEnumerable<string> Files { get; }
}