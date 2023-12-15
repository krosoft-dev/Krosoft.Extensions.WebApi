using MediatR;

namespace Krosoft.Extensions.Samples.Library.Models.Commands;

public class LogicielUpdateCommand : LogicielBaseCommand<Unit>
{
    public Guid Id { get; set; }
}