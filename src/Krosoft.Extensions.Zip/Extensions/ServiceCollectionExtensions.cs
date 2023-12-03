using Krosoft.Extensions.Zip.Interfaces;
using Krosoft.Extensions.Zip.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Zip.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZip(this IServiceCollection services)
    {
        services.AddTransient<IZipService, ZipService>();

        return services;
    }
}