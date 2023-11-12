using MediatR;

namespace Krosoft.Extensions.Core.Cqrs.Models.Commands;

public interface ICommand : IRequest
{
}

public interface ICommand<out T> : IRequest<T>
{
}