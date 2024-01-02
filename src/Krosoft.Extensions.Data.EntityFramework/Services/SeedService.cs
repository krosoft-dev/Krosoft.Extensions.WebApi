using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Services;

public abstract class SeedService<TDbContext> : ISeedService<TDbContext> where TDbContext : DbContext
{
    public bool Initialized { get; set; }

    public void InitializeDbForTests(TDbContext db)
    {
        Initialized = true;

        BeforeSave(db);

        db.SaveChanges();
    }

    protected virtual void BeforeSave(TDbContext db)
    {
    }
}