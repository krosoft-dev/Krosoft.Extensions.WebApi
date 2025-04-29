using Krosoft.Extensions.Samples.Library.Models.Commands;
using Krosoft.Extensions.Samples.Library.Models.Queries;
using Krosoft.Extensions.WebApi.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Endpoints;

internal class JobsEndpoint : IEndpoint
{
    public void Register(RouteGroupBuilder group)
    {
        group.MapGet("/", (IMediator mediator,
                           CancellationToken cancellationToken)
                         => mediator.Send(new JobsQuery(), cancellationToken));
        group.MapPost("/Run/{identifiant}", (IMediator mediator,
                                             [FromRoute] string identifiant,
                                             CancellationToken cancellationToken) =>
                          mediator.Send(new JobTriggerCommand(identifiant), cancellationToken));
    }

    public RouteGroupBuilder DefineGroup(WebApplication app) => app.MapGroup("/Jobs")
                                                                   .DisableAntiforgery()
                                                                   .WithTags("Jobs");
}