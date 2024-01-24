using System.Net;
using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.WebApi.Blocking.Middlewares;

public class BlockingMiddleware : IMiddleware
{
    private readonly IAccessTokenBlockingService _accessTokenBlockingService;
    private readonly IIdentifierBlockingService _identifierBlockingService;
    private readonly IIpBlockingService _ipBlockingService;
    private readonly ILogger<BlockingMiddleware> _logger;

    public BlockingMiddleware(ILogger<BlockingMiddleware> logger,
                              IAccessTokenBlockingService accessTokenBlockingService,
                              IIdentifierBlockingService identifierBlockingService,
                              IIpBlockingService ipBlockingService)
    {
        _logger = logger;
        _accessTokenBlockingService = accessTokenBlockingService;
        _identifierBlockingService = identifierBlockingService;
        _ipBlockingService = ipBlockingService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var remoteIp = context.Connection.RemoteIpAddress;
        var isIpBlocked = await _ipBlockingService.IsBlockedAsync(remoteIp?.ToString() ?? string.Empty, context.RequestAborted);
        if (isIpBlocked)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }

        var endpoint = context.GetEndpoint();

        if (endpoint is null)
        {
            await next(context);
            return;
        }

        _logger.LogDebug($"Endpoint : {endpoint.DisplayName}");

        if (endpoint is RouteEndpoint routeEndpoint)
        {
            _logger.LogDebug($"Route Pattern : {routeEndpoint.RoutePattern.RawText}");
        }

        if (endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null)
        {
            await next(context);
            return;
        }

        if (endpoint.Metadata.GetMetadata<AuthorizeAttribute>() != null)
        {
            var isIdentifierBlocked = await _identifierBlockingService.IsBlockedAsync(context.RequestAborted);
            if (isIdentifierBlocked)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            var isAccessTokenBlocked = await _accessTokenBlockingService.IsBlockedAsync(context.RequestAborted);
            if (isAccessTokenBlocked)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
        }

        await next(context);
    }
}