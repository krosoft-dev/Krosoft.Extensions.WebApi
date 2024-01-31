using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Data;

public class SampleKrosoftAuditableContext : KrosoftAuditableContext
{
    public SampleKrosoftAuditableContext(DbContextOptions options,
                                         IAuditableDbContextProvider auditableDbContextProvider)
        : base(options, auditableDbContextProvider)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDataJson<Statistique>();
        builder.HasDataJson<Logiciel>();
        builder.HasDataJson<Langue>();
        builder.HasDataJson<Pays>();
    }
}