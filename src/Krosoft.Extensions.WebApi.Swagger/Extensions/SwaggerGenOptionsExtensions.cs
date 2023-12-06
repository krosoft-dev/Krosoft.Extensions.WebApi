using Krosoft.Extensions.WebApi.Swagger.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Krosoft.Extensions.WebApi.Swagger.Extensions;

public static class SwaggerGenOptionsExtensions
{
    public static SwaggerGenOptions AddSecurityBearer(this SwaggerGenOptions options)
    {
        const string openApiReferenceId = "Bearer-Token";
        options.AddSecurityDefinition(openApiReferenceId, new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please provide a valid token",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });

        options.OperationFilter<AuthenticationRequirementsOperationFilter>(openApiReferenceId);

        return options;
    }

    public static SwaggerGenOptions AddSecurityApiKey(this SwaggerGenOptions options)
    {
        const string openApiReferenceId = "ApiKey-Token";

        options.AddSecurityDefinition(openApiReferenceId, new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please provide a valid token",
            Name = HeaderNames.Authorization,
            Type = SecuritySchemeType.ApiKey
        });

        options.OperationFilter<AuthenticationRequirementsOperationFilter>(openApiReferenceId);

        return options;
    }
}