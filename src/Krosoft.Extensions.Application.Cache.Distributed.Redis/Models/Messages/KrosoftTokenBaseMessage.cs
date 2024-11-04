namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Messages;

public abstract class KrosoftTokenBaseMessage
{
    public string? UtilisateurId { get; set; }
    public string? TenantId { get; set; }
}