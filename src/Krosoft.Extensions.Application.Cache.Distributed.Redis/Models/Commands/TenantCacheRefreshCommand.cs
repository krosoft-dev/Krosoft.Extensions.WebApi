using Krosoft.Extensions.Cqrs.Models.Commands;
using MediatR;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Commands;

public class TenantCacheRefreshCommand : AuthBaseCommand<Unit>
{
    public TenantCacheRefreshCommand(bool isAuthenticationRequired, bool isTenantRequired)
    {
        IsUtilisateurRequired = isAuthenticationRequired;
        IsTenantRequired = isTenantRequired;
    }

    public TenantCacheRefreshCommand(bool isAuthenticationRequired)
        : this(isAuthenticationRequired, false)
    {
    }

    public override bool IsUtilisateurRequired { get; }
    public override bool IsTenantRequired { get; }
}