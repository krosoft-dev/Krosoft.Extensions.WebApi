using System.Globalization;
using Krosoft.Extensions.Core.Models.Exceptions;
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

        builder.UseAuthentication();
        builder.UseAuthorization();
        builder.UseMiddlewares(true);

        if (actionBuilder != null)
        {
            actionBuilder(builder);
        }

        builder.UseCultures(configuration);
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

    public static IApplicationBuilder UseCultures(this IApplicationBuilder builder, IConfiguration configuration)
    {
        var configurationSection = configuration.GetSection("AppSettings:Cultures");
        if (configurationSection == null)
        {
            throw new KrosoftTechniqueException("Impossible de trouver la clé AppSettings:Cultures dans la configuration.");
        }

        var cultures = configurationSection.Get<string[]>();
        if (cultures != null && cultures.Any())
        {
            IList<CultureInfo> supportedCultures = new List<CultureInfo>();

            foreach (var culture in cultures)
            {
                supportedCultures.Add(new CultureInfo(culture));
            }

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(cultures[0]),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };
            builder.UseRequestLocalization(localizationOptions);
        }

        return builder;
    }
}