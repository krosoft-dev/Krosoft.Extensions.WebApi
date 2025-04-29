using Krosoft.Extensions.WebApi.Identity.Attributes;
using Microsoft.AspNetCore.Builder;

namespace Krosoft.Extensions.WebApi.Identity.Extensions;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder RequireApiKey(this RouteHandlerBuilder builder)
    {
        builder.WithMetadata(new RequireApiKeyAttribute());
        return builder;
    }

    public static RouteHandlerBuilder RequireAgentId(this RouteHandlerBuilder builder)
    {
        builder.WithMetadata(new RequireAgentIdAttribute());
        return builder;
    }
}