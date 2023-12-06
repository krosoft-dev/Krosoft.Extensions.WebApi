using System.Reflection;
using Krosoft.Extensions.WebApi.Swagger.Filters;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Krosoft.Extensions.WebApi.Swagger.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services,
                                                Assembly assembly,
                                                Action<SwaggerGenOptions>? setupAction = null)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            if (setupAction != null)
            {
                setupAction(options);
            }

            options.DocumentFilter<EnumDescriptionFilter>();
            options.SchemaFilter<SwaggerExcludeSchemaFilter>();
            options.OperationFilter<SwaggerExcludeOperationFilter>();
            options.EnableAnnotations(true, true);
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml"));

            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml"));
        });

        return services;
    }
}