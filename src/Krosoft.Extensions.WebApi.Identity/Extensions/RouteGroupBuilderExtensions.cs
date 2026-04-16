#if NET9_0_OR_GREATER
using Krosoft.Extensions.WebApi.Identity.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
#endif

namespace Krosoft.Extensions.WebApi.Identity.Extensions;

public static class RouteGroupBuilderExtensions
{
#if NET9_0_OR_GREATER
    public static RouteGroupBuilder MapTaggedGroup(this WebApplication app,
                                                   string pattern,
                                                   string? tag = null)
    {
        var derivedTag = tag ?? Regex.Replace(pattern.TrimStart('/'), @"/\{[^}]+\}", "", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        return app.MapGroup(pattern).WithTags(derivedTag);
    }

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
#endif
}