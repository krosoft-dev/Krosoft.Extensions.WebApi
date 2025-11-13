using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Documents.DeposerFichier;

public record DeposerFichierSansRetourCommand(
    long FichierId,
    KrosoftFile? File)
    : BaseCommand;