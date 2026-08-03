using System.Net;
using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.WebApi.Blocking.Middlewares;

/// <summary>
///     Rejette les requêtes présentant un access token bloqué.
///     Contrairement à <see cref="BlockingMiddleware" />, seules les requêtes porteuses d'un access token sont
///     contrôlées : une API peut aussi être consommée par clé d'API, auquel cas il n'y a pas de token à vérifier.
/// </summary>
public class AccessTokenBlockingMiddleware : IMiddleware
{
    private readonly IAccessTokenBlockingService _accessTokenBlockingService;
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly ILogger<AccessTokenBlockingMiddleware> _logger;

    public AccessTokenBlockingMiddleware(ILogger<AccessTokenBlockingMiddleware> logger,
                                         IAccessTokenBlockingService accessTokenBlockingService,
                                         IAccessTokenProvider accessTokenProvider)
    {
        _logger = logger;
        _accessTokenBlockingService = accessTokenBlockingService;
        _accessTokenProvider = accessTokenProvider;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var accessToken = await _accessTokenProvider.GetAccessTokenAsync(context.RequestAborted);
        if (string.IsNullOrEmpty(accessToken))
        {
            await next(context);
            return;
        }

        var isAccessTokenBlocked = await _accessTokenBlockingService.IsBlockedAsync(accessToken, context.RequestAborted);
        if (isAccessTokenBlocked)
        {
            _logger.LogWarning("Access token bloqué : requête refusée.");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }

        await next(context);
    }
}
