using Krosoft.Extensions.Cqrs.Models.Commands;
using MediatR;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Commands;

public class AuthCacheRefreshCommand : AuthBaseCommand<Unit>
{
    public AuthCacheRefreshCommand(bool isAuthenticationRequired, bool isTenantRequired)
    {
        IsUtilisateurRequired = isAuthenticationRequired;
        IsTenantRequired = isTenantRequired;
    }

    public AuthCacheRefreshCommand(bool isAuthenticationRequired)
        : this(isAuthenticationRequired, false)
    {
    }

    public override bool IsUtilisateurRequired { get; }
    public override bool IsTenantRequired { get; }
}