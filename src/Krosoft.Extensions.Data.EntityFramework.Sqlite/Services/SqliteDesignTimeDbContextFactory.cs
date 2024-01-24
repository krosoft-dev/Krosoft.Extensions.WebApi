using Krosoft.Extensions.Data.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Sqlite.Services;

public abstract class SqliteDesignTimeDbContextFactory<TDbContext> : DesignTimeDbContextFactory<TDbContext> where TDbContext : DbContext
{
    protected override void UseProvider(DbContextOptionsBuilder options, string connectionString)
    {
        options.UseSqlite(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(typeof(TDbContext).Assembly.FullName));
    }
}