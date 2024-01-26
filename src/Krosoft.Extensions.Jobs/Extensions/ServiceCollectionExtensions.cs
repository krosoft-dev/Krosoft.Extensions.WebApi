using Krosoft.Extensions.Jobs.Interfaces;
using Krosoft.Extensions.Jobs.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Jobs.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFireForget(this IServiceCollection services)
    {
        services.AddTransient<IFireForgetService, FireForgetService>();

        return services;
    }
}