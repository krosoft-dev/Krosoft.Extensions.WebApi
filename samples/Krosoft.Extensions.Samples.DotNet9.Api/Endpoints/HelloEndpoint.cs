using Krosoft.Extensions.Samples.DotNet9.Api.Models.Dto;
using Krosoft.Extensions.Samples.Library.Models.Commands;
using Krosoft.Extensions.WebApi.Interfaces;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Endpoints;

internal class HelloEndpoint : IEndpoint
{
    public void Register(RouteGroupBuilder group)
    {
        group.MapPost("/", (HelloDotNet9CommandDto dto,
                            IMediator mediator,
                            CancellationToken cancellationToken)
                          => mediator.Send(new HelloDotNet9Command(dto.Name), cancellationToken));
    }

    public RouteGroupBuilder DefineGroup(WebApplication app) => app.MapGroup("/Hello")
                                                                   .DisableAntiforgery()
                                                                   .WithTags("Hello");
}