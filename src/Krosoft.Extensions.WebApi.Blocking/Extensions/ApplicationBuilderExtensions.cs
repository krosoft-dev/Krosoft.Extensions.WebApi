using Krosoft.Extensions.WebApi.Blocking.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Krosoft.Extensions.WebApi.Blocking.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseBlocking(this IApplicationBuilder app)
        => app.UseMiddleware<BlockingMiddleware>();

    /// <summary>
    ///     Contrôle uniquement le blocage des access tokens, et seulement pour les requêtes qui en portent un.
    ///     À utiliser avec <c>AddAccessTokenBlocking</c>.
    /// </summary>
    public static IApplicationBuilder UseAccessTokenBlocking(this IApplicationBuilder app)
        => app.UseMiddleware<AccessTokenBlockingMiddleware>();
}