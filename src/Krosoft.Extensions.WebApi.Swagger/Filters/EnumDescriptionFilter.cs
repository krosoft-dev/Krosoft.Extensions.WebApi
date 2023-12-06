using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Krosoft.Extensions.WebApi.Swagger.Filters;

public class EnumDescriptionFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc,
                      DocumentFilterContext context)
    {
        // Add enum descriptions to result models.
        foreach (var property in swaggerDoc.Components.Schemas)
        {
            IList<IOpenApiAny> enums = property.Value.Enum;
            if (enums != null && enums.Count > 0)
            {
                property.Value.Description += DescribeEnum(enums, property.Key);
            }
        }

        // Add enum descriptions to input parameters.
        foreach (var pathItem in swaggerDoc.Paths.Values)
        {
            DescribeEnumParameters(pathItem.Operations, swaggerDoc);
        }
    }

    private static void DescribeEnumParameters(IDictionary<OperationType, OpenApiOperation>? operations,
                                               OpenApiDocument swaggerDoc)
    {
        if (operations != null)
        {
            foreach (var operation in operations)
            {
                foreach (var parameter in operation.Value.Parameters)
                {
                    var schemas = swaggerDoc.Components.Schemas.FirstOrDefault(x => x.Key == parameter.Name);
                    if (schemas.Value != null)
                    {
                        parameter.Description += DescribeEnum(schemas.Value.Enum, schemas.Key);
                    }
                }
            }
        }
    }

    private static Type? GetEnumTypeByName(string enumTypeName)
    {
        return AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany(x => x.GetTypes())
                        .FirstOrDefault(x => x.Name == enumTypeName);
    }

    private static string? DescribeEnum(IEnumerable<IOpenApiAny> enums, string proprtyTypeName)
    {
        var enumType = GetEnumTypeByName(proprtyTypeName);
        if (enumType != null)
        {
            var enumDescriptions = new List<string>();

            foreach (var openApiAny in enums)
            {
                if (openApiAny is OpenApiInteger openApiInteger)
                {
                    var enumAsInt = openApiInteger.Value;

                    enumDescriptions.Add($"{enumAsInt} = {Enum.GetName(enumType, enumAsInt)}");
                }
            }

            return string.Join(", ", enumDescriptions.ToArray());
        }

        return null;
    }
}