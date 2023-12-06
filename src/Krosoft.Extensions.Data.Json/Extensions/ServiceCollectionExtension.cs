using Krosoft.Extensions.Data.Json.Interfaces;
using Krosoft.Extensions.Data.Json.Models;
using Krosoft.Extensions.Data.Json.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.Json.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddJsonDataService(this IServiceCollection services,
                                                        IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<JsonDataSettings>(configuration.GetSection(nameof(JsonDataSettings)));
        services.AddScoped(typeof(IJsonDataService<>), typeof(JsonDataService<>));
        return services;
    }
}