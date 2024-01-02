namespace Krosoft.Extensions.Data.Abstractions.Models;

public abstract class TenantAuditableEntity : AuditableEntity, ITenant
{
    public string? TenantId { get; set; }
}