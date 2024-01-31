using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Data.EntityFramework.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.PostgreSql.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContextPostgreSql<TDbContext>(this IServiceCollection services,
                                                                        IConfiguration configuration,
                                                                        string dbContextName) where TDbContext : DbContext

    {
        Guard.IsNotNull(nameof(dbContextName), dbContextName);

        var connectionString = configuration.GetConnectionString(dbContextName);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new KrosoftTechniqueException($"La ConnectionString basé sur '{dbContextName}' n'est pas définie.");
        }

        services.AddDbContextPostgreSql<TDbContext>(connectionString);

        return services;
    }

    public static IServiceCollection AddDbContextPostgreSql<TDbContext>(this IServiceCollection services,
                                                                        string connectionString) where TDbContext : DbContext

    {
        Guard.IsNotNull(nameof(connectionString), connectionString);

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddScoped<DbContext, TDbContext>();
        services.AddDbContext<TDbContext>(options =>
                                              options.UseLoggerFactory(LoggerFactoryHelper.MyLoggerFactory)
                                                     .UseNpgsql(connectionString)
                                                     .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        return services;
    }

    public static IServiceCollection AddDbContextPostgreSql<TDbContext>(this IServiceCollection services,
                                                                        IConfiguration configuration) where TDbContext : DbContext

    {
        var dbContextName = typeof(TDbContext).Name;
        var connectionString = configuration.GetConnectionString(dbContextName);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new KrosoftTechniqueException($"La ConnectionString basé sur '{dbContextName}' n'est pas définie.");
        }

        services.AddDbContextPostgreSql<TDbContext>(connectionString);

        return services;
    }
}