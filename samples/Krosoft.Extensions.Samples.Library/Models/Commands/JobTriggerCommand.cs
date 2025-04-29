using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.Library.Models.Commands;

public record JobTriggerCommand(string Identifiant) : BaseCommand;