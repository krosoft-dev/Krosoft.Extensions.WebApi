namespace Krosoft.Extensions.Cqrs.Models.Commands;

public abstract record BaseCommand : ICommand;

public abstract record BaseCommand<T> : ICommand<T>;