using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.Library.Models.Commands;

public record HelloDotNet9Command(string Name) : BaseCommand<string>;