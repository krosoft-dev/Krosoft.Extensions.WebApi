using System.Reflection;
using Krosoft.Extensions.Core.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Extensions;

public static class ModelBuilderExtensions
{
    public static void HasDataJson<TEntity>(this ModelBuilder builder, Assembly assembly) where TEntity : class
    {
        builder.Entity<TEntity>().HasData(JsonHelper.Get<TEntity>(assembly));
    }

    public static void HasDataJson<TEntity>(this ModelBuilder builder) where TEntity : class
    {
        builder.Entity<TEntity>().HasData(JsonHelper.Get<TEntity>(typeof(TEntity).Assembly));
    }
}