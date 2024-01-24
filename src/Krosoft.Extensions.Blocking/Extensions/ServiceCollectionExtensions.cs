using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Krosoft.Extensions.Blocking.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Blocking.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlocking(this IServiceCollection services)
    {
        services.AddSingleton<IAccessTokenBlockingService, AccessTokenBlockingService>();
        services.AddSingleton<IIdentifierBlockingService, IdentifierBlockingService>();
        services.AddSingleton<IIpBlockingService, IpBlockingService>();

        return services;
    }
}