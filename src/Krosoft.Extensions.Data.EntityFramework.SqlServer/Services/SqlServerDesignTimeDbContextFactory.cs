using Krosoft.Extensions.Data.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.SqlServer.Services
{
    public abstract class SqlServerDesignTimeDbContextFactory<TDbContext> : DesignTimeDbContextFactory<TDbContext> where TDbContext : DbContext
    {
        protected override void UseProvider(DbContextOptionsBuilder options, string connectionString)
        {
            options.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(typeof(TDbContext).Assembly.FullName));
        }
    }
}