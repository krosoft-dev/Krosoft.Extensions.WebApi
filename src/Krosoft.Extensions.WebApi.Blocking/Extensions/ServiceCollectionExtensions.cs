using Krosoft.Extensions.Blocking.Extensions;
using Krosoft.Extensions.WebApi.Blocking.Middlewares;
using Krosoft.Extensions.WebApi.Identity.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.WebApi.Blocking.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWepApiBlocking(this IServiceCollection services)
    {
        services.AddBlocking();
        services.AddAccessTokenProvider();
        services.AddIdentifierProvider();
        services.AddTransient<BlockingMiddleware>();

        return services;
    }
}