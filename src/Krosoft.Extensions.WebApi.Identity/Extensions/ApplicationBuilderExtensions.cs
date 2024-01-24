using Krosoft.Extensions.WebApi.Identity.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Krosoft.Extensions.WebApi.Identity.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseTenant(this IApplicationBuilder app)
        => app.UseMiddleware<MissingTenantMiddleware>();
}