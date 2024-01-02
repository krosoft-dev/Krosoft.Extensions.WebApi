using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Krosoft.Extensions.Data.Abstractions.Models;

namespace Krosoft.Extensions.Data.EntityFramework.Extensions;

public static class ChangeTrackerExtensions
{
    public static void ProcessCreationTenant(this ChangeTracker changeTracker,
                                             string tenantId)
    {
        foreach (var item in changeTracker.Entries<ITenant>().Where(e => e.State == EntityState.Added))
        {
            item.Entity.TenantId = tenantId;
        }
    }

    public static void ProcessCreationAuditable(this ChangeTracker changeTracker,
                                                DateTime now,
                                                string utilisateurId)
    {
        foreach (var item in changeTracker.Entries<IAuditable>()
                                          .Where(e => e.State == EntityState.Added))
        {
            item.Entity.CreateurId = utilisateurId;
            item.Entity.CreateurDate = now;
            item.Entity.ModificateurId = utilisateurId;
            item.Entity.ModificateurDate = now;
        }
    }

    public static void ProcessModificationAuditable(this ChangeTracker changeTracker,
                                                    DateTime now,
                                                    string utilisateurId)
    {
        foreach (var item in changeTracker.Entries<IAuditable>()
                                          .Where(e => e.State == EntityState.Modified))
        {
            item.Entity.ModificateurId = utilisateurId;
            item.Entity.ModificateurDate = now;
        }
    }
}