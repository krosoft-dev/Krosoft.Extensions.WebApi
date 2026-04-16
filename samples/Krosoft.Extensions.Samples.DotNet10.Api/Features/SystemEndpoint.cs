using Krosoft.Extensions.Samples.DotNet10.Api.Features.Hello.Get;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features;

internal class SystemEndpoint : IEndpoint
{
    public RouteGroupBuilder DefineGroup(WebApplication app) =>
        app.MapTaggedGroup("/System/{id}/Configuration");

    public void Register(RouteGroupBuilder group)
    {
        group.MapGet("/", ([FromRoute] Guid id,
                           IMediator mediator,
                           CancellationToken cancellationToken)
                         => mediator.Send(new SystemConfigurationQuery(id), cancellationToken));
    }
}