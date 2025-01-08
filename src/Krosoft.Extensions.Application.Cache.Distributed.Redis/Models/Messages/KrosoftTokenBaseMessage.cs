namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Messages;

public abstract record KrosoftTokenBaseMessage
{
    public string? UtilisateurId { get; set; }
    public string? TenantId { get; set; }
}