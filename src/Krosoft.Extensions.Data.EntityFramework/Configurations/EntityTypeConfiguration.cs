using Krosoft.Extensions.Data.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Krosoft.Extensions.Data.EntityFramework.Configurations;

public abstract class EntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
    where T : Entity
{
    private readonly string? _schema;
    private readonly string _tableName;

    protected EntityTypeConfiguration(string tableName)
        : this(null, tableName)
    {
    }

    protected EntityTypeConfiguration(string? schema, string tableName)
    {
        _tableName = tableName;
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(_tableName, _schema);
        ConfigureMore(builder);
    }

    protected abstract void ConfigureMore(EntityTypeBuilder<T> builder);
}