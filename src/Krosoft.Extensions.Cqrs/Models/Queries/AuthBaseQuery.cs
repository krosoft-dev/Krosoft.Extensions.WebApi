namespace Krosoft.Extensions.Cqrs.Models.Queries;

public record AuthBaseQuery<TResponse> : BaseQuery<TResponse>, IAuthQuery<TResponse>
{
    public virtual bool IsTenantRequired => true;

    public virtual bool IsUtilisateurRequired => true;
    public string? TenantId { get; set; }
    public string? UtilisateurCourantId { get; set; }
}