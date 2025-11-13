using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Cqrs.Models.Commands;
using Krosoft.Extensions.Samples.DotNet10.Api.Features.Documents.DeposerFichier;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Documents.DeposerFichierSansRetour;

public record DeposerFichierCommand(
    long FichierId,
    KrosoftFile? File)
    : BaseCommand<DepotDto>;