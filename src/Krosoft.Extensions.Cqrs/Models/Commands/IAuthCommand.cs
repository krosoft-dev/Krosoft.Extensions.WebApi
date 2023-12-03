namespace Krosoft.Extensions.Cqrs.Models.Commands;

public interface IAuthCommand : ICommand, IAuth
{
}

public interface IAuthCommand<out T> : ICommand<T>, IAuth
{
}