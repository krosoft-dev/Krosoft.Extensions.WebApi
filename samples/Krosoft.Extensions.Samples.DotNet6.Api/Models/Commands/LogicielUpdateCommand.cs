using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Models.Commands;

public class LogicielUpdateCommand : LogicielBaseCommand<Unit>, ICommand
{
    public Guid Id { get; set; }
}