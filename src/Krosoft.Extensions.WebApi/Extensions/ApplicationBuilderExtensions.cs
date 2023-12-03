using System.Globalization;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseWebApi(this IApplicationBuilder builder,
                                                IWebHostEnvironment env,
                                                IConfiguration configuration) =>
        builder.UseWebApi(env, configuration, null);

    public static IApplicationBuilder UseWebApi(this IApplicationBuilder builder,
                                                IWebHostEnvironment env,
                                                IConfiguration configuration,
                                                Action<IApplicationBuilder>? action)
    {
        if (env.IsDevelopment())
        {
            builder.UseDeveloperExceptionPage();
        }

        builder.UseRouting();

        builder.UseCors(b => b.AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowAnyOrigin());

        builder.UseAuthentication();
        builder.UseAuthorization();
        builder.UseMiddlewares(true);
        if (action != null)
        {
            action(builder);
        }

        //builder.UseHealthChecksExt(env);
        builder.UseCultures(configuration);
        builder.UseResponseCompression();

        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            //endpoints.MapHealthChecksExt();
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