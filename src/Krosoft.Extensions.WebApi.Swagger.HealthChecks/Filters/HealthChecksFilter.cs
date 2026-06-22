#if NET10_0_OR_GREATER
using Microsoft.OpenApi;
#else
using Microsoft.OpenApi.Models;
#endif
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.WebApi.HealthChecks.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Krosoft.Extensions.WebApi.Swagger.HealthChecks.Filters;

public class HealthChecksFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)

    {
        var pathItem = CreateOpenApiPathItem();

        swaggerDoc.Paths.Add(Urls.Health.Check, pathItem);
        swaggerDoc.Paths.Add(Urls.Health.Readiness, pathItem);
        swaggerDoc.Paths.Add(Urls.Health.Liveness, pathItem);
    }

    private static OpenApiPathItem CreateOpenApiPathItem()
    {
        var response = new OpenApiResponse
        {
            Description = "Success"
        };
        response.Content.Add(HttpClientExtensions.MediaTypeJson, new OpenApiMediaType

        {
            Schema = new OpenApiSchema
            {
                Type = "object",
                AdditionalPropertiesAllowed = true,
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    { "status", new OpenApiSchema { Type = "string" } }
                }
            }
        });

        var operation = new OpenApiOperation();
        operation.Tags.Add(new OpenApiTag { Name = nameof(Urls.Health) });
        operation.Responses.Add("200", response);

        var pathItem = new OpenApiPathItem();
        pathItem.AddOperation(OperationType.Get, operation);
        return pathItem;
    }
}