#if NET9_0_OR_GREATER
using Krosoft.Extensions.WebApi.Identity.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
#endif

namespace Krosoft.Extensions.WebApi.Identity.Extensions;

public static class RouteGroupBuilderExtensions
{
#if NET9_0_OR_GREATER
    public static RouteGroupBuilder RequireApiKey(this RouteGroupBuilder builder)
    {
        builder.WithMetadata(new RequireApiKeyAttribute());
        return builder;
    }

    public static RouteGroupBuilder RequireAgentId(this RouteGroupBuilder builder)
    {
        builder.WithMetadata(new RequireAgentIdAttribute());
        return builder;
    }

    public static RouteGroupBuilder RequirePermission(this RouteGroupBuilder group,
                                                      string? roles = null)
    {
        group.DisableAntiforgery();
        return roles is null
            ? group.RequireAuthorization()
            : group.RequireAuthorization(new AuthorizeAttribute { Roles = roles });
    }

    /// <summary>
    /// Surcharge route par route : <c>MapGet</c>/<c>MapPost</c>/… renvoient un <see cref="RouteHandlerBuilder" />
    /// (et non un <see cref="RouteGroupBuilder" />), on peut ainsi exiger une permission sur une seule route
    /// sans passer par un sous-groupe.
    /// </summary>
    public static RouteHandlerBuilder RequirePermission(this RouteHandlerBuilder builder,
                                                        string? roles = null)
    {
        builder.DisableAntiforgery();
        return roles is null
            ? builder.RequireAuthorization()
            : builder.RequireAuthorization(new AuthorizeAttribute { Roles = roles });
    }
#endif
}