using Krosoft.Extensions.WebApi.Swagger.HealthChecks.Filters;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Krosoft.Extensions.WebApi.Swagger.HealthChecks.Extensions;

public static class SwaggerGenOptionsExtensions
{
    public static SwaggerGenOptions AddHealthChecks(this SwaggerGenOptions options)
    {
        options.DocumentFilter<HealthChecksFilter>();

        return options;
    }
}