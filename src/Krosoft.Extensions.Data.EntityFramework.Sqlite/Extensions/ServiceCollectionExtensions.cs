using Krosoft.Extensions.Data.EntityFramework.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.Sqlite.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContextSqlite<TDbContext>(this IServiceCollection services,
                                                                    IConfiguration configuration,
                                                                    string dbContextName) where TDbContext : DbContext

    {
        services.AddDbContextSqlite<TDbContext>(configuration.GetConnectionString(dbContextName));

        return services;
    }

    public static IServiceCollection AddDbContextSqlite<TDbContext>(this IServiceCollection services,
                                                                    string connectionString) where TDbContext : DbContext

    {
        services.AddScoped<DbContext, TDbContext>();
        services.AddDbContext<TDbContext>(options =>
                                              options.UseLoggerFactory(LoggerFactoryHelper.MyLoggerFactory)
                                                     .UseSqlite(connectionString)
                                                     .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        return services;
    }

    public static IServiceCollection AddDbContextSqlite<TDbContext>(this IServiceCollection services,
                                                                    IConfiguration configuration) where TDbContext : DbContext

    {
        var dbContextName = typeof(TDbContext).Name;

        services.AddDbContextSqlite<TDbContext>(configuration.GetConnectionString(dbContextName));

        return services;
    }
}