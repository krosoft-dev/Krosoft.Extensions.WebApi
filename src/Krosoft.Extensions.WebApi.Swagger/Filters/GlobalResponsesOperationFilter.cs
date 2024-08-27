using System.Net;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.WebApi.Swagger.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Krosoft.Extensions.WebApi.Swagger.Filters;

public class GlobalResponsesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        Add(operation, context, HttpStatusCode.NotFound, typeof(ErrorDto));
        Add(operation, context, HttpStatusCode.BadRequest, typeof(ErrorDto));
        Add(operation, context, HttpStatusCode.InternalServerError, typeof(ErrorDto));

        if (context.IsAuthRequired())
        {
            Add(operation, context, HttpStatusCode.Unauthorized, typeof(ErrorDto));
        }
    }

    private static void Add(OpenApiOperation operation, OperationFilterContext context, HttpStatusCode responseStatusCode, Type type)
    {
        var statusCode = ((int)responseStatusCode).ToString();

        operation.Responses.TryAdd(statusCode, new OpenApiResponse
        {
            Description = responseStatusCode.ToString().AddSpacesBeforeCapitals(),
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository)
                }
            }
        });
    }
}