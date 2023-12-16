using System.Globalization;
using Krosoft.Extensions.WebApi.Middlewares;
using Krosoft.Extensions.WebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCors(this IApplicationBuilder builder, WebApiSettings? webApiSettings)
    {
        if (webApiSettings != null)
        {
            if (webApiSettings.AllowedOrigins.Any() || webApiSettings.ExposedHeaders.Any())
            {
                builder.UseCors(policy =>
                {
                    policy.AllowAnyMethod();

                    if (webApiSettings.AllowedOrigins.Any())
                    {
                        policy.WithOrigins(webApiSettings.AllowedOrigins);
                    }
                    else
                    {
                        policy.AllowAnyOrigin();
                    }

                    if (webApiSettings.ExposedHeaders.Any())
                    {
                        policy.WithExposedHeaders(webApiSettings.ExposedHeaders);
                    }
                    else
                    {
                        policy.AllowAnyHeader();
                    }
                });
            }
        }

        return builder;
    }

    public static IApplicationBuilder UseCultures(this IApplicationBuilder builder, WebApiSettings? webApiSettings)
    {
        if (webApiSettings != null)
        {
            if (webApiSettings.Cultures.Any())

            {
                IList<CultureInfo> supportedCultures = new List<CultureInfo>();

                foreach (var culture in webApiSettings.Cultures)
                {
                    supportedCultures.Add(new CultureInfo(culture));
                }

                var localizationOptions = new RequestLocalizationOptions
                {
                    DefaultRequestCulture = new RequestCulture(webApiSettings.Cultures[0]),
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures
                };
                builder.UseRequestLocalization(localizationOptions);
            }
        }

        return builder;
    }

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder builder, bool useTenant)
    {
        if (useTenant)
        {
            //builder.UseMiddleware<MissingTenantMiddleware>();
        }

        return builder
               .UseMiddleware<CustomExceptionHandlerMiddleware>()
               .UseMiddleware<RequestLoggingMiddleware>();
    }

    public static IApplicationBuilder UseWebApi(this IApplicationBuilder builder,
                                                IWebHostEnvironment env,
                                                IConfiguration configuration,
                                                Action<IApplicationBuilder>? actionBuilder = null,
                                                Action<IEndpointRouteBuilder>? actionEndpoints = null)
    {
        if (env.IsDevelopment())
        {
            builder.UseDeveloperExceptionPage();
        }

        builder.UseRouting();

        var webApiSettings = new WebApiSettings();
        configuration.GetSection(nameof(WebApiSettings)).Bind(webApiSettings);

        builder.UseCors(webApiSettings);
        builder.UseAuthentication();
        builder.UseAuthorization();
        builder.UseMiddlewares(true);

        if (actionBuilder != null)
        {
            actionBuilder(builder);
        }

        builder.UseCultures(webApiSettings);
        builder.UseResponseCompression();

        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            if (actionEndpoints != null)
            {
                actionEndpoints(endpoints);
            }
        });
        return builder;
    }
}