using Krosoft.Extensions.WebApi.Blocking.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Krosoft.Extensions.WebApi.Blocking.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseBlocking(this IApplicationBuilder app)
        => app.UseMiddleware<BlockingMiddleware>();
}