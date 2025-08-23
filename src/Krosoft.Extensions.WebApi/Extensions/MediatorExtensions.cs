using Krosoft.Extensions.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class MediatorExtensions
{
    public static async Task<TResponse> SendWithFileAsync<TResponse>(this IMediator mediator,
                                                                     IFormFile formFile,
                                                                     Func<KrosoftFile?, IRequest<TResponse>> func,
                                                                     CancellationToken cancellationToken)
    {
        var file = await formFile.ToFileAsync(cancellationToken);
        var command = func(file);
        return await mediator.Send(command, cancellationToken);
    }

    public static async Task SendWithFileAsync(this IMediator mediator,
                                               IFormFile formFile,
                                               Func<KrosoftFile?, IRequest> func,
                                               CancellationToken cancellationToken = default)
    {
        var file = await formFile.ToFileAsync(cancellationToken);
        var command = func(file);
        await mediator.Send(command, cancellationToken);
    }
}