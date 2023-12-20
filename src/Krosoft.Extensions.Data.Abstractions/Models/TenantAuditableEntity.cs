namespace Krosoft.Extensions.Data.Abstractions.Models;

public abstract class TenantAuditableEntity : AuditableEntity, ITenantId
{
    public string? TenantId { get; set; }
}