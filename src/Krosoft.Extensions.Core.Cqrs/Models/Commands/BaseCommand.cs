namespace Krosoft.Extensions.Core.Cqrs.Models.Commands;

public abstract class BaseCommand : ICommand
{
}

public abstract class BaseCommand<T> : ICommand<T>
{
}