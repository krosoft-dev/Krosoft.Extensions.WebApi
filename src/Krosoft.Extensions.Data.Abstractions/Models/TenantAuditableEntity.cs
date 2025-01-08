namespace Krosoft.Extensions.Data.Abstractions.Models;

public abstract record TenantAuditableEntity : AuditableEntity, ITenant
{
    public string? TenantId { get; set; }
}