using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Krosoft.Extensions.Blocking.Abstractions.Models;
using Krosoft.Extensions.Blocking.Extensions;
using Krosoft.Extensions.Blocking.Services;
using Krosoft.Extensions.WebApi.Blocking.Middlewares;
using Krosoft.Extensions.WebApi.Identity.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.WebApi.Blocking.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWepApiBlocking(this IServiceCollection services,
                                                       Action<KrosoftBlockingOptions> action)
    {
        services.AddBlocking(action);
        services.AddAccessTokenProvider();
        services.AddIdentifierProvider();
        services.AddTransient<BlockingMiddleware>();

        return services;
    }

    /// <summary>
    ///     Enregistre le blocage des seuls access tokens, sans les blocages par IP et par identifiant.
    ///     À utiliser avec <c>UseAccessTokenBlocking</c>.
    /// </summary>
    public static IServiceCollection AddAccessTokenBlocking(this IServiceCollection services,
                                                            Action<KrosoftBlockingOptions> action)
    {
        services.AddBlockingStorage(action);
        services.AddAccessTokenProvider();
        services.AddSingleton<IAccessTokenBlockingService, AccessTokenBlockingService>();
        services.AddTransient<AccessTokenBlockingMiddleware>();

        return services;
    }
}
