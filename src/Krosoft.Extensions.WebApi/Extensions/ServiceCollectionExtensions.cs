using System.IO.Compression;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApi(this IServiceCollection services,
                                               Assembly assembly,
                                               IConfiguration configuration)
    {
        services.AddAuthorization();
        services.AddCors();
        services.AddControllers();

        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });

#if NET6_0_OR_GREATER
        services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.SmallestSize; });
#else
        services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });
#endif

        services.AddHttpContextAccessor();
        services.AddLocalization();
        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());

        services.AddLogging(opt =>
        {
            opt.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.SingleLine = true;
                options.TimestampFormat = "dd-MM-yyyy hh:mm:ss ";
            });
        });

        //Services.
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly, typeof(ServiceCollectionExtensions).Assembly));

        return services;
    }
}