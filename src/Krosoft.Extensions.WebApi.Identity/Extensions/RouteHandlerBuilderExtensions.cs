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
}