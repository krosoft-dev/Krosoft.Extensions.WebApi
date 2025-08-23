using System.Security.Claims;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Constantes;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Abstractions.Models;
using Krosoft.Extensions.Identity.Extensions;
using Krosoft.Extensions.WebApi.Identity.Helpers;
using Krosoft.Extensions.WebApi.Identity.Interface;
using Krosoft.Extensions.WebApi.Identity.Middlewares;
using Krosoft.Extensions.WebApi.Identity.Models;
using Krosoft.Extensions.WebApi.Identity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
#if NET8_0_OR_GREATER
using Microsoft.AspNetCore.Http;
#endif

namespace Krosoft.Extensions.WebApi.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccessTokenProvider(this IServiceCollection services)
    {
        services.AddTransient<IAccessTokenProvider, HttpAccessTokenProvider>();

        return services;
    }

    public static IServiceCollection AddApiKey(this IServiceCollection services,
                                               IConfiguration configuration) =>
        services.Configure<WebApiIdentySettings>(configuration.GetSection(nameof(WebApiIdentySettings)))
                .AddTransient<IApiKeyValidator, SettingsApiKeyValidator>()
                .AddApiKeyProvider()
                .AddApiKeyStorage();

    public static IServiceCollection AddApiKeyProvider(this IServiceCollection services)
    {
        services.AddTransient<IApiKeyProvider, HttpApiKeyProvider>();

        return services;
    }

    public static IServiceCollection AddAgentIdProvider(this IServiceCollection services)
    {
        services.AddTransient<IAgentIdProvider, HttpAgentIdProvider>();

        return services;
    }

    public static IServiceCollection AddApiKeyStorage(this IServiceCollection services)
    {
        services.AddTransient<IApiKeyStorageProvider, SettingsApiKeyStorageProvider>();

        return services;
    }

    public static IServiceCollection AddIdentifierProvider(this IServiceCollection services)
    {
        services.AddTransient<IJwtTokenValidator, JwtTokenValidator>();
        services.AddTransient<IIdentifierProvider, HttpIdentifierProvider>();

        return services;
    }

    public static IServiceCollection AddJwtWithApiKeyAuthentication<T>(this IServiceCollection services,
                                                                       IConfiguration configuration)
        where T : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        services.AddOptions();
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        if (jwtSettings == null)
        {
            throw new KrosoftTechnicalException($"Impossible d'instancier l'objet de type '{nameof(JwtSettings)}'.");
        }

        var signingCredentials = SigningCredentialsHelper.GetSigningCredentials(jwtSettings.SecurityKey);
        services.AddAuthentication(authenticationOptions =>
                {
                    authenticationOptions.DefaultAuthenticateScheme = "Combined";
                    authenticationOptions.DefaultChallengeScheme = "Combined";
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
                {
                    jwtBearerOptions.RequireHttpsMetadata = false;
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.TokenValidationParameters = IdentitTokenHelper.GetTokenValidationParameters(signingCredentials, jwtSettings, true);
                    jwtBearerOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = OnAuthenticationFailed,
                        OnTokenValidated = OnTokenValidated
                    };
                })
                .AddScheme<ApiKeyAuthenticationOptions, T>("ApiKey", _ => { })
                .AddPolicyScheme("Combined", "JWT or ApiKey", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        if (context.Request.Headers.ContainsKey(ApiKeyMiddleware.ApiKeyHeaderName))
                        {
                            return "ApiKey";
                        }

                        return JwtBearerDefaults.AuthenticationScheme;
                    };
                });

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
                                                          IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        if (jwtSettings == null)
        {
            throw new KrosoftTechnicalException($"Impossible d'instancier l'objet de type '{nameof(JwtSettings)}'.");
        }

        var signingCredentials = SigningCredentialsHelper.GetSigningCredentials(jwtSettings.SecurityKey);
        services.AddAuthentication(authenticationOptions =>
                {
                    authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.RequireHttpsMetadata = false;
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.TokenValidationParameters = IdentitTokenHelper.GetTokenValidationParameters(signingCredentials, jwtSettings, true);
                    jwtBearerOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = OnAuthenticationFailed,
                        OnTokenValidated = OnTokenValidated
                    };
                });

        return services;
    }

    public static IServiceCollection AddJwtGenerator(this IServiceCollection services,
                                                     IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.AddDateTimeService();
        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddTransient<IJwtTokenValidator, JwtTokenValidator>();
        services.AddRefreshTokenGenerator();

        return services;
    }

    public static IServiceCollection AddWebApiIdentityEx(this IServiceCollection services)
    {
        services.AddTransient<IClaimsService, HttpClaimsService>();
        services.AddIdentityEx();

        return services;
    }

    private static Task OnAuthenticationFailed(AuthenticationFailedContext context)
    {
        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
        {
#if NET8_0_OR_GREATER
            context.Response.Headers.Append("Token-Expired", "true");
#else
            if (!context.Response.Headers.ContainsKey("Token-Expired"))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
#endif
        }

        return Task.CompletedTask;
    }

    private static Task OnTokenValidated(TokenValidatedContext context)
    {
        var claims = new List<Claim>();
        if (context.Principal != null)
        {
            var claimsPermissions = context.Principal.Claims
                                           .Where(claim => claim.Type == KrosoftClaimNames.Permissions)
                                           .Select(claim => new Claim(ClaimTypes.Role, claim.Value));
            claims.AddRange(claimsPermissions);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            context.Principal.AddIdentity(claimsIdentity);
        }

        return Task.CompletedTask;
    }
}