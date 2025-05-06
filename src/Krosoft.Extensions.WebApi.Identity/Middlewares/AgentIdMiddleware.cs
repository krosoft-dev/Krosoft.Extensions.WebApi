using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.Identity.Attributes;
using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.Identity.Middlewares;

public class AgentIdMiddleware
{
    public const string AgentIdHeaderName = "x-agent-id";

    private readonly RequestDelegate _next;

    public AgentIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        var requiresAgentId = endpoint?.Metadata.GetMetadata<RequireAgentIdAttribute>() != null;

        if (!requiresAgentId)
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(AgentIdHeaderName, out _))
        {
            await context.HandleExceptionAsync(new UnauthorizedException("Agent Id was not provided."));
            return;
        }

        await _next(context);
    }
}