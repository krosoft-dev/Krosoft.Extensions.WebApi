using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Abstractions.Models;
using Krosoft.Extensions.Identity.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if NET8_0_OR_GREATER
#endif

namespace Krosoft.Extensions.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityEx(this IServiceCollection services)
    {
        services.AddTransient<IClaimsBuilderService, ClaimsBuilderService>();
        services.AddTransient<IKrosoftTokenBuilderService, KrosoftTokenBuilderService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddPasswordHasher();

        return services;
    }

    public static IServiceCollection AddPasswordHasher(this IServiceCollection services)
    {
        services.AddTransient<ISimplePasswordHasher, SimplePasswordHasher>();

        return services;
    }

    public static IServiceCollection AddRefreshTokenGenerator(this IServiceCollection services)
    {
        services.AddTransient<IRefreshTokenGenerator, RefreshTokenGenerator>();

        return services;
    }

    public static IServiceCollection AddTokenProvider(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<TokenSettings>(configuration.GetSection(nameof(TokenSettings)));
        services.AddDataProtection();
        services.AddTransient<ITokenProvider, TokenProvider>();

        return services;
    }
}