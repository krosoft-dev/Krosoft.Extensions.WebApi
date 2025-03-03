using System.Diagnostics;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Cqrs.Models;
using Krosoft.Extensions.Cqrs.Models.Commands;
using Krosoft.Extensions.Cqrs.Models.Queries;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Cqrs.Behaviors.Identity.PipelineBehaviors;

public class ApiKeyPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IApiKeyProvider _apiKeyProvider;
    private readonly ILogger<ApiKeyPipelineBehavior<TRequest, TResponse>> _logger;

    public ApiKeyPipelineBehavior(ILogger<ApiKeyPipelineBehavior<TRequest, TResponse>> logger,
                                  IApiKeyProvider apiKeyProvider)
    {
        _logger = logger;
        _apiKeyProvider = apiKeyProvider;
    }

    public async Task<TResponse> Handle(TRequest request,
                                        RequestHandlerDelegate<TResponse> next,
                                        CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        _logger.LogInformation($"Handling ApiKeyPipelineBehavior <{typeof(TRequest).Name},{typeof(TResponse).Name}>");

        switch (request)
        {
            case IAuth auth:
                if (auth.IsUtilisateurRequired)
                {
                    auth.UtilisateurCourantId = await _apiKeyProvider.GetApiKeyAsync(cancellationToken);
                }

                break;

            case IQuery<TResponse>:
            case ICommand<TResponse>:
            case ICommand:
                break;

            default:
                throw new KrosoftTechnicalException($"Le type {typeof(TRequest)} n'est pas pris en compte pour cet appel.");
        }

        _logger.LogInformation($"Handled ApiKeyPipelineBehavior <{typeof(TRequest).Name},{typeof(TResponse).Name}> in {sw.Elapsed.ToShortString()}");
        return await next();
    }
}