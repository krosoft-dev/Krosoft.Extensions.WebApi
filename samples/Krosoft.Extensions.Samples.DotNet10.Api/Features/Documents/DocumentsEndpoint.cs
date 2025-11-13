using Krosoft.Extensions.Samples.DotNet9.Api.Features.Documents.DeposerFichier;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Documents.DeposerFichierSansRetour;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.Interfaces;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Documents;

internal class DocumentsEndpoint : IEndpoint
{
    public RouteGroupBuilder DefineGroup(WebApplication app) =>
        app.MapGroup("/Documents")
           .DisableAntiforgery()
           .WithTags("Documents");

    public void Register(RouteGroupBuilder group)
    {
        group.MapPost("/Deposer/Fichier", ([AsParameters] DeposerFichierDto dto,
                                           IMediator mediator,
                                           CancellationToken cancellationToken)
                          => mediator.SendWithFileAsync(dto.File,
                                                        file => new DeposerFichierCommand(dto.FichierId, file),
                                                        cancellationToken));

        group.MapPost("/Deposer/Fichier/SansRetour", ([AsParameters] DeposerFichierDto dto,
                                                      IMediator mediator,
                                                      CancellationToken cancellationToken)
                          => mediator.SendWithFileAsync(dto.File,
                                                        file => new DeposerFichierSansRetourCommand(dto.FichierId, file),
                                                        cancellationToken));
    }
}