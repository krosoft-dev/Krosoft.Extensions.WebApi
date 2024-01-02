using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;

namespace Krosoft.Extensions.Data.EntityFramework.Contexts;

public abstract class KrosoftContext : DbContext
{
    /// <summary>
    /// Find loaded entity types from assemblies that application uses.
    /// </summary>
    private static IEnumerable<Type>? _entityTypeCache;

    protected KrosoftContext(DbContextOptions options) : base(options)
    {
    }

    protected IEnumerable<Type> GetEntityTypes()
    {
        if (_entityTypeCache != null)
        {
            return _entityTypeCache.ToList();
        }

        var types = GetTypes().ToList();

        if (types.Any())
        {
            _entityTypeCache = (from a in GetReferencingAssemblies()
                                from t in a.DefinedTypes
                                where !t.IsAbstract && t.GetInterfaces().Any(types.Contains)
                                select t.AsType())
                               .DistinctBy(x => x.FullName)
                               .ToList();

            return _entityTypeCache;
        }

        return new List<Type>();
    }

    private static List<Assembly> GetReferencingAssemblies()
    {
        var assemblies = new List<Assembly>();
        if (DependencyContext.Default != null)
        {
            var dependencies = DependencyContext.Default.RuntimeLibraries;

            foreach (var library in dependencies)
            {
                try
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
                catch (FileNotFoundException)
                {
                }
            }
        }

        return assemblies;
    }

    protected virtual IEnumerable<Type> GetTypes() => new List<Type>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        foreach (var relationship in builder.Model.GetEntityTypes()
                                            .Where(e => !e.IsOwned())
                                            .SelectMany(e => e.GetForeignKeys()))
        {
            if (relationship.DeleteBehavior == DeleteBehavior.Cascade)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }

    protected virtual void OverrideEntities()
    {
    }

    public override int SaveChanges()
    {
        OverrideEntities();

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OverrideEntities();

        return await base.SaveChangesAsync(true, cancellationToken);
    }
}