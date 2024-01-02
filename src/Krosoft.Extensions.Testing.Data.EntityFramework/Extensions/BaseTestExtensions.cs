using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.Abstractions.Models;
using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Testing.Data.EntityFramework.Extensions;

public static class BaseTestExtensions
{
    public static IQueryable<T> GetDb<T>(this BaseTest baseTest, IServiceProvider container) where T : Entity
    {
        var context = container.GetRequiredService<IReadRepository<T>>();

        return context.Query();
    }

    public static void InitDb<TDbContext, T>(this BaseTest baseTest,
                                             IServiceProvider container,
                                             T entity) where TDbContext : KrosoftContext where T : Entity
    {
        baseTest.InitDb<TDbContext, T>(container, new List<T> { entity });
    }

    public static void InitDb<TDbContext, T>(this BaseTest baseTest,
                                             IServiceProvider container,
                                             IEnumerable<T> entities) where TDbContext : KrosoftContext where T : Entity
    {
        var context = container.GetRequiredService<TDbContext>();

        context.AddRange(entities);
        context.SaveChanges();
    }
}