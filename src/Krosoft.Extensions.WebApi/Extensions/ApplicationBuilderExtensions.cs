using System.Globalization;
using Krosoft.Extensions.WebApi.Middlewares;
using Krosoft.Extensions.WebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
    private static readonly string FORWARDED_PREFIX_HEADER = "X-Forwarded-Prefix";

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

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder builder) =>
        builder
            .UseMiddleware<CustomExceptionHandlerMiddleware>()
            .UseMiddleware<RequestLoggingMiddleware>();

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

        builder.Use((context, next) =>
        {
            var pathBase = context.Request.Headers[FORWARDED_PREFIX_HEADER].FirstOrDefault();
            if (!string.IsNullOrEmpty(pathBase))
            {
                context.Request.PathBase = new PathString(pathBase);
            }

            return next();
        });

        var webApiSettings = new WebApiSettings();
        configuration.GetSection(nameof(WebApiSettings)).Bind(webApiSettings);

        builder.UseRouting();

        if (webApiSettings.AllowedOrigins.Any() || webApiSettings.ExposedHeaders.Any())
        {
            builder.UseCors();
        }

        builder.UseAuthentication();
        builder.UseAuthorization();
        builder.UseMiddlewares();

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