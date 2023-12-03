namespace Krosoft.Extensions.Cqrs.Models.Queries;

public class AuthBaseQuery<TResponse> : BaseQuery<TResponse>, IAuthQuery<TResponse>
{
    public string? UtilisateurCourantId { get; set; }
    public string? TenantId { get; set; }

    public virtual bool IsUtilisateurRequired => true;
    public virtual bool IsTenantRequired => true;
}