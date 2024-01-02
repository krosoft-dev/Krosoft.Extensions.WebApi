using System.Reflection;
using Krosoft.Extensions.Data.Abstractions.Models;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Contexts;

public abstract class KrosoftAuditableContext : KrosoftContext
{
    private static readonly MethodInfo ConfigureAuditableMethod = typeof(KrosoftAuditableContext)
                                                                  .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                                  .Single(t => t.IsGenericMethod && t.Name == nameof(ConfigureAuditable));

    private readonly IAuditableDbContextProvider _auditableDbContextProvider;

    protected KrosoftAuditableContext(DbContextOptions options,
                                      IAuditableDbContextProvider auditableDbContextProvider) : base(options)
    {
        _auditableDbContextProvider = auditableDbContextProvider;
    }

    public void ConfigureAuditable<T>(ModelBuilder builder) where T : class, IAuditable
    {
        builder.Entity<T>()
               .Property(t => t.ModificateurId)
               .IsRequired();
        builder.Entity<T>()
               .Property(t => t.ModificateurDate)
               .IsRequired();
        builder.Entity<T>()
               .Property(t => t.CreateurId)
               .IsRequired();
        builder.Entity<T>()
               .Property(t => t.CreateurDate)
               .IsRequired();
    }

    protected override IEnumerable<Type> GetTypes() => [typeof(IAuditable)];

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Set BaseEntity rules to all loaded entity types
        foreach (var type in GetEntityTypes())
        {
            //Console.WriteLine(type.FullName); //Debug.

            if (type.GetInterfaces().Contains(typeof(IAuditable)))
            {
                var method = ConfigureAuditableMethod.MakeGenericMethod(type);
                method.Invoke(this, new object[] { builder });
            }
        }
    }

    protected override void OverrideEntities()
    {
        var useAudit = ChangeTracker.Entries<IAuditable>().Any();
        if (useAudit)
        {
            ChangeTracker.DetectChanges();

            var now = _auditableDbContextProvider.GetNow();
            var utilisateurId = _auditableDbContextProvider.GetUtilisateurId();

            ChangeTracker.ProcessModificationAuditable(now, utilisateurId);
            ChangeTracker.ProcessCreationAuditable(now, utilisateurId);
        }
    }
}