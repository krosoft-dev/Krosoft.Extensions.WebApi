using MediatR;

namespace Krosoft.Extensions.Core.Cqrs.Models.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}