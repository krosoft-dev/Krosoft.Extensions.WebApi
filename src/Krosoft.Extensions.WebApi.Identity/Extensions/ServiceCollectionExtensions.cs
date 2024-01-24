using System.Security.Claims;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Constantes;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Abstractions.Models;
using Krosoft.Extensions.Identity.Extensions;
using Krosoft.Extensions.WebApi.Identity.Helpers;
using Krosoft.Extensions.WebApi.Identity.Services;
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

    public static IServiceCollection AddIdentifierProvider(this IServiceCollection services)
    {
        services.AddTransient<IJwtTokenValidator, JwtTokenValidator>();
        services.AddTransient<IIdentifierProvider, HttpIdentifierProvider>();

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
            throw new KrosoftTechniqueException($"Impossible d'instancier l'objet de type '{nameof(JwtSettings)}'.");
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
            var claimsDroits = context.Principal.Claims
                                      .Where(claim => claim.Type == KrosoftClaimNames.Droits)
                                      .Select(claim => new Claim(ClaimTypes.Role, claim.Value));
            claims.AddRange(claimsDroits);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            context.Principal.AddIdentity(claimsIdentity);
        }

        return Task.CompletedTask;
    }
}