using Krosoft.Extensions.Data.Abstractions.Models;

namespace Krosoft.Extensions.Samples.Library.Models.Entities;

public class Pays : AuditableEntity
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
}