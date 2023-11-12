namespace Krosoft.Extensions.Core.Cqrs.Models.Queries;

public interface IAuthQuery<out TResponse> : IQuery<TResponse>, IAuth
{
}