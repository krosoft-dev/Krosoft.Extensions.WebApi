using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Krosoft.Extensions.WebApi.Swagger.Extensions;

public static class OperationFilterContextExtensions
{
    public static bool IsAuthRequired(this OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType == null)
        {
            return false;
        }

        var globalAttributes = context.ApiDescription.ActionDescriptor.FilterDescriptors.Select(p => p.Filter);
        var controlerAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true);
        var methodAttributes = context.MethodInfo.GetCustomAttributes(true);
        var anonymousAttributes = globalAttributes
                                  .Union(controlerAttributes)
                                  .Union(methodAttributes)
                                  .OfType<AllowAnonymousAttribute>();

        var noAuthRequired = anonymousAttributes.Any();
        return !noAuthRequired;
    }
}