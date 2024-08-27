using Krosoft.Extensions.WebApi.Swagger.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Krosoft.Extensions.WebApi.Swagger.Filters;

public class AuthenticationRequirementsOperationFilter : IOperationFilter
{
    private readonly string _openApiReferenceId;

    public AuthenticationRequirementsOperationFilter(string openApiReferenceId)
    {
        _openApiReferenceId = openApiReferenceId;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.IsAuthRequired())
        {
            if (operation.Security == null)
            {
                operation.Security = new List<OpenApiSecurityRequirement>();
            }

            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = _openApiReferenceId
                }
            };
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
        }
    }
}