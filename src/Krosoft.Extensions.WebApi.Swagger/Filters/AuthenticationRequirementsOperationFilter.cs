using Microsoft.AspNetCore.Authorization;
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
        var globalAttributes = context.ApiDescription.ActionDescriptor.FilterDescriptors.Select(p => p.Filter);
        if (context.MethodInfo.DeclaringType != null)
        {
            var controlerAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true);
            var methodAttributes = context.MethodInfo.GetCustomAttributes(true);
            var anonymousAttributes = globalAttributes
                                      .Union(controlerAttributes)
                                      .Union(methodAttributes)
                                      .OfType<AllowAnonymousAttribute>();

            var noAuthRequired = anonymousAttributes.Any();
            if (!noAuthRequired)
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
}