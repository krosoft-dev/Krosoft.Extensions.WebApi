using Krosoft.Extensions.Reporting.Csv.Interfaces;
using Krosoft.Extensions.Reporting.Csv.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Reporting.Csv.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCsv(this IServiceCollection services)
    {
        services.AddTransient<ICsvReadService, CsvReadService>();

        return services;
    }
}