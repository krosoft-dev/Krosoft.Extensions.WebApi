using MediatR;

namespace Krosoft.Extensions.Samples.Library.Models.Commands;

public record LogicielUpdateCommand : LogicielBaseCommand<Unit>
{
    public Guid Id { get; set; }
}