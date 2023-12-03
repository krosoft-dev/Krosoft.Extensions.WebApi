namespace Krosoft.Extensions.Cqrs.Models.Queries;

public interface IAuthQuery<out TResponse> : IQuery<TResponse>, IAuth
{
}