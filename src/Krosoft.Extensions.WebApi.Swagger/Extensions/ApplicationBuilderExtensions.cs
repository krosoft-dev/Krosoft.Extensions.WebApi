using Microsoft.AspNetCore.Builder;

namespace Krosoft.Extensions.WebApi.Swagger.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerExt(this IApplicationBuilder builder,
                                                    bool useSwagger = true)
    {
        if (useSwagger)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI();
        }

        return builder;
    }
}