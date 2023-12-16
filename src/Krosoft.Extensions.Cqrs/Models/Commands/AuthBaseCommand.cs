namespace Krosoft.Extensions.Cqrs.Models.Commands;

public abstract class AuthBaseCommand : BaseCommand, IAuthCommand
{
    public virtual bool IsTenantRequired => true;

    public virtual bool IsUtilisateurRequired => true;
    public string? TenantId { get; set; }
    public string? UtilisateurCourantId { get; set; }
}

public abstract class AuthBaseCommand<T> : BaseCommand<T>, IAuthCommand<T>
{
    public virtual bool IsTenantRequired => true;
    public virtual bool IsUtilisateurRequired => true;
    public string? TenantId { get; set; }

    public string? UtilisateurCourantId { get; set; }
}