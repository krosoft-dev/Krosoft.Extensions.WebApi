namespace Krosoft.Extensions.Data.Abstractions.Models;

public abstract record AuditableEntity : Entity, IAuditable
{
    public string? ModificateurId { get; set; }
    public DateTime ModificateurDate { get; set; }
    public string? CreateurId { get; set; }
    public DateTime CreateurDate { get; set; }
}