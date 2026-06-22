using System.Reflection;
using Krosoft.Extensions.Core.Attributes;
#if NET10_0_OR_GREATER
using Microsoft.OpenApi;
#else
using Microsoft.OpenApi.Models;
#endif
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Krosoft.Extensions.WebApi.Swagger.Filters;

public class SwaggerExcludeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation? operation, OperationFilterContext? context)
    {
        if (operation == null || context == null)
        {
            return;
        }

        if (operation.Parameters == null || context.MethodInfo == null)
        {
            return;
        }

        if (operation.Parameters.Count == 0)
        {
            return;
        }

        var ignoredProperties = context.MethodInfo
                                       .GetParameters()
                                       .SelectMany(p => p.ParameterType.GetProperties()
                                                         .Where(prop => prop.GetCustomAttribute<SwaggerExcludePropertyAttribute>() != null))
                                       .Select(m =>
                                       {
                                           var s = m.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
                                           if (s != null)
                                           {
                                               return s;
                                           }

                                           return m.Name;
                                       })
                                       .ToList();

        foreach (var name in ignoredProperties)
        {
            operation.Parameters = operation.Parameters
                                            .Where(p => !p.Name.Equals(name, StringComparison.InvariantCulture))
                                            .ToList();
        }
    }
}