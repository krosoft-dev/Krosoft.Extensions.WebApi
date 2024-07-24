using System.IO.Compression;
using System.Reflection;
using FluentValidation;
using Krosoft.Extensions.WebApi.Interfaces;
using Krosoft.Extensions.WebApi.Models;
using Krosoft.Extensions.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCompression(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });

#if NET6_0_OR_GREATER
        services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.SmallestSize; });
#else
        services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });
#endif
        return services;
    }

    public static IServiceCollection AddCors(this IServiceCollection services, WebApiSettings? webApiSettings)
    {
        if (webApiSettings != null && (webApiSettings.AllowedOrigins.Any() || webApiSettings.ExposedHeaders.Any()))
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {                
                    if (webApiSettings.AllowedOrigins.Length > 0)
                    {
                        policy.WithOrigins(webApiSettings.AllowedOrigins);
                    }
                    else
                    {
                        policy.AllowAnyOrigin();
                    }

                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                    policy.AllowCredentials();

                    if (webApiSettings.ExposedHeaders.Length > 0)
                    {
                        policy.WithExposedHeaders(webApiSettings.ExposedHeaders);
                    }
                });
            });
        }

        return services;
    }

    public static IServiceCollection AddCorsPolicyAccessor(this IServiceCollection services)
    {
        services.AddTransient<ICorsPolicyAccessor, CorsPolicyAccessor>();
        return services;
    }

    public static IServiceCollection AddLoggingExt(this IServiceCollection services)
    {
        services.AddLogging(opt =>
        {
            opt.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.SingleLine = true;
                options.TimestampFormat = "dd-MM-yyyy hh:mm:ss ";
            });
        });

        return services;
    }

    public static IServiceCollection AddWebApi(this IServiceCollection services,
                                               IConfiguration configuration,
                                               params Assembly[] assemblies)
    {
        services.AddAuthorization();
        services.AddOptions();

        var webApiSettings = new WebApiSettings();
        configuration.GetSection(nameof(WebApiSettings)).Bind(webApiSettings);
        services.Configure<WebApiSettings>(configuration.GetSection(nameof(WebApiSettings)));

        services.AddCors(webApiSettings);
        services.AddControllers();
        services.AddCompression();

        services.AddHttpContextAccessor();
        services.AddLocalization();
        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());

        services.AddLoggingExt();

        var all = new List<Assembly> { typeof(ServiceCollectionExtensions).Assembly };
        all.AddRange(assemblies);

        //Services. 
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(all.ToArray()));
        services.AddAutoMapper(all);
        services.AddValidatorsFromAssemblies(all, includeInternalTypes: true);
        return services;
    }
}