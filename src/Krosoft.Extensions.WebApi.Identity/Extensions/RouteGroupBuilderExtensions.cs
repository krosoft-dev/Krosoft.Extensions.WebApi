#if NET9_0_OR_GREATER
using Krosoft.Extensions.WebApi.Identity.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
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

#endif
}