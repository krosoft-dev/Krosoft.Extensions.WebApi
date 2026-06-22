using Microsoft.AspNetCore.Builder;

namespace Krosoft.Extensions.WebApi.Swagger.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerExt(this IApplicationBuilder builder,
                                                    bool useSwagger = true)
    {
        if (useSwagger)
        {
#if NET10_0_OR_GREATER
   builder.UseSwagger(options =>
            {
                options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;
            });
#else
            builder.UseSwagger();
#endif

            builder.UseSwaggerUI();
        }

        return builder;
    }
}