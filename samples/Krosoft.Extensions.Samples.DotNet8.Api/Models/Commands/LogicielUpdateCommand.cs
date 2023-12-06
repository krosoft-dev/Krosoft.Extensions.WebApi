using Krosoft.Extensions.Cqrs.Models.Commands;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Models.Commands;

public class LogicielUpdateCommand : LogicielBaseCommand<Unit>, ICommand
{
    public Guid Id { get; set; }
}