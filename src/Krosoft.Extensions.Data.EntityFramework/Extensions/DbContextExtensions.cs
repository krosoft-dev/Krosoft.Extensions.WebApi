using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Data.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Extensions;

public static class DbContextExtensions
{
    public static void Import<T>(this DbContext db) where T : Entity
    {
        var entities = JsonHelper.Get<T>(typeof(T).Assembly);
        db.Set<T>().AddRange(entities);
    }
}