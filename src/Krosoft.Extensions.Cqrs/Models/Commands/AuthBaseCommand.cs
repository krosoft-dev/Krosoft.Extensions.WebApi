namespace Krosoft.Extensions.Cqrs.Models.Commands;

public abstract class AuthBaseCommand : BaseCommand, IAuthCommand
{
    public string? UtilisateurCourantId { get; set; }
    public string? TenantId { get; set; }

    public virtual bool IsUtilisateurRequired => true;
    public virtual bool IsTenantRequired => true;
}

public abstract class AuthBaseCommand<T> : BaseCommand<T>, IAuthCommand<T>
{
    public virtual bool IsUtilisateurRequired => true;
    public virtual bool IsTenantRequired => true;

    public string? UtilisateurCourantId { get; set; }
    public string? TenantId { get; set; }
}