using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.WebApi.Identity.Services;

/// <summary>
/// Donne un corps aux 401/403 produits par le middleware d'autorisation (ex. <c>RequirePermission</c>),
/// au même format que les autres erreurs Krosoft (<c>ErrorDto</c>). Un 403 indique la permission manquante.
/// Le challenge et le forbid des schémas d'authentification sont conservés (en-têtes <c>WWW-Authenticate</c>…).
/// </summary>
public class PermissionAuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
{
    /// <summary>Clé de <c>HttpContext.Items</c> où est déposé le message d'erreur (ex. pour un journal des appels).</summary>
    public const string ErrorItemKey = "Krosoft.AuthorizationError";

    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();
    private readonly PermissionErrorOptions _options;

    public PermissionAuthorizationResultHandler(IOptions<PermissionErrorOptions> options)
    {
        _options = options.Value;
    }

    public async Task HandleAsync(RequestDelegate next,
                                  HttpContext context,
                                  AuthorizationPolicy policy,
                                  PolicyAuthorizationResult authorizeResult)
    {
        // Comportement standard d'abord : appel de l'endpoint si autorisé, sinon challenge / forbid des schémas.
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);

        if (context.Response.HasStarted)
        {
            return;
        }

        if (authorizeResult.Challenged)
        {
            var message = _options.UnauthorizedMessage(context);
            context.Items[ErrorItemKey] = message;
            await context.HandleExceptionAsync(new UnauthorizedException(message));
        }
        else if (authorizeResult.Forbidden)
        {
            var message = GetForbiddenMessage(authorizeResult);
            context.Items[ErrorItemKey] = message;
            await context.HandleExceptionAsync(new ForbiddenException(message));
        }
    }

    /// <summary>Permission(s) manquante(s) : une exigence de rôle est satisfaite par l'un quelconque de ses rôles.</summary>
    private string GetForbiddenMessage(PolicyAuthorizationResult authorizeResult)
    {
        var requirements = authorizeResult.AuthorizationFailure?.FailedRequirements
                                          .OfType<RolesAuthorizationRequirement>()
                                          .Select(r => string.Join(" ou ", r.AllowedRoles))
                                          .Where(r => r.Length > 0)
                                          .Distinct()
                                          .ToList() ?? [];

        return requirements.Count > 0
                   ? $"Permission requise : {string.Join(", ", requirements)}."
                   : _options.ForbiddenMessage;
    }
}
