using MediatR;

namespace Krosoft.Extensions.Cqrs.Models.Commands;

public interface ICommand : IRequest;

public interface ICommand<out T> : IRequest<T>;