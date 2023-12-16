using System.Reflection;
using Krosoft.Extensions.Core.Attributes;
using Krosoft.Extensions.Core.Extensions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Krosoft.Extensions.WebApi.Swagger.Filters;

public class SwaggerExcludeSchemaFilter : ISchemaFilter
{
    private const BindingFlags Flags = BindingFlags.Public |
                                       BindingFlags.NonPublic |
                                       BindingFlags.Instance;

    public void Apply(OpenApiSchema? schema, SchemaFilterContext? context)
    {
        if (schema == null || context == null)
        {
            return;
        }

        if (schema.Properties == null || context.Type == null)
        {
            return;
        }

        if (schema.Properties.Count == 0)
        {
            return;
        }

        var properties = context.Type.GetFields(Flags)
                                .Cast<MemberInfo>()
                                .Concat(context.Type.GetProperties(Flags));

        var ignoredProperties = properties.Where(m => m.GetCustomAttribute<SwaggerExcludePropertyAttribute>() != null)
                                          .Select(m =>
                                          {
                                              var s = m.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
                                              if (s != null)
                                              {
                                                  return s;
                                              }

                                              return m.Name.ToCamelCase();
                                          })
                                          .ToList();

        foreach (var name in ignoredProperties)
        {
            if (name != null && schema.Properties.ContainsKey(name))
            {
                schema.Properties.Remove(name);
            }
        }
    }
}