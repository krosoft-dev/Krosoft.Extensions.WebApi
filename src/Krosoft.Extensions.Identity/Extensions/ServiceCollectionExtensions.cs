using System.Security.Claims;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Constantes;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Abstractions.Models;
using Krosoft.Extensions.Identity.Helpers;
using Krosoft.Extensions.Identity.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
#if NET8_0_OR_GREATER
using Microsoft.AspNetCore.Http;
#endif

namespace Krosoft.Extensions.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTokenProvider(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<TokenSettings>(configuration.GetSection(nameof(TokenSettings)));
        services.AddDataProtection();
        services.AddTransient<ITokenProvider, TokenProvider>();

        return services;
    }

    public static IServiceCollection AddIdentityEx(this IServiceCollection services)
    {
        services.AddTransient<IClaimsService, ClaimsService>();
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

    public static IServiceCollection AddJwtGenerator(this IServiceCollection services,
                                                     IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddTransient<IRefreshTokenGenerator, RefreshTokenGenerator>();

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

        if (string.IsNullOrEmpty(jwtSettings.SecurityKey))
        {
            throw new KrosoftTechniqueException($"'{nameof(jwtSettings.SecurityKey)}' non définie.");
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
                        OnAuthenticationFailed = context =>
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
                        },

                        OnTokenValidated = context =>
                        {
                            var claims = new List<Claim>();
                            if (context.Principal != null)
                            {
                                foreach (var claim in context.Principal.Claims)
                                {
                                    if (claim.Type == KrosoftClaimNames.Droits)
                                    {
                                        claims.Add(new Claim(ClaimTypes.Role, claim.Value));
                                    }
                                }

                                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                context.Principal.AddIdentity(claimsIdentity);
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

        return services;
    }
}