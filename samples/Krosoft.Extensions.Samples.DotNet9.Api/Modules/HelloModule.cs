using Krosoft.Extensions.Samples.Library.Models.Queries;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Modules;

internal static class HelloModule
{
    internal static Task<string> Hello(IMediator mediator,
                                       CancellationToken cancellationToken) =>
        mediator.Send(new HelloDotNet9Query(), cancellationToken);

    internal static Task<string> HelloApiKey(IMediator mediator,
                                             CancellationToken cancellationToken) =>
        mediator.Send(new HelloDotNet9Query(), cancellationToken);
}