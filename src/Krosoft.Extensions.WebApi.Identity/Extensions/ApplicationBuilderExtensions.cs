using Krosoft.Extensions.WebApi.Identity.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Krosoft.Extensions.WebApi.Identity.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseTenant(this IApplicationBuilder app)
        => app.UseMiddleware<MissingTenantMiddleware>();

    public static IApplicationBuilder UseApiKey(this IApplicationBuilder app)
        => app.UseMiddleware<ApiKeyMiddleware>();

    public static IApplicationBuilder UseAgentId(this IApplicationBuilder app)
        => app.UseMiddleware<AgentIdMiddleware>();
}