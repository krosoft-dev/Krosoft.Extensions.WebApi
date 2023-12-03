using MediatR;

namespace Krosoft.Extensions.Cqrs.Models.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}