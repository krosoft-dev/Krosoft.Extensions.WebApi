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

    private static IEnumerable<Type> GetDefinedTypes(ISet<Type> types)
    {
        var definedTypes = new List<Type>();
        if (DependencyContext.Default != null)
        {
            var dependencies = DependencyContext.Default.RuntimeLibraries;

            foreach (var library in dependencies)
            {
                try
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    definedTypes.AddRange(assembly.DefinedTypes
                                                  .Where(t => !t.IsAbstract && t.GetInterfaces().Any(types.Contains))
                                                  .Select(t => t)
                                                  .DistinctBy(x => x.FullName)
                                                  .ToHashSet());
                }
                catch (FileNotFoundException)
                {
                }
                catch (ReflectionTypeLoadException)
                {
                }
            }
        }

        return definedTypes;
    }

    protected IEnumerable<Type> GetEntityTypes()
    {
        if (_entityTypeCache != null)
        {
            return _entityTypeCache.ToList();
        }

        var types = GetTypes().ToHashSet();
        if (types.Any())
        {
            _entityTypeCache = GetDefinedTypes(types);

            return _entityTypeCache;
        }

        return new List<Type>();
    }

    protected virtual IEnumerable<Type> GetTypes() => new List<Type>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
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