using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.Identity.Models;

/// <summary>
/// Messages des réponses 401/403 produites par <c>PermissionAuthorizationResultHandler</c>.
/// </summary>
public class PermissionErrorOptions
{
    /// <summary>Message d'un 401 (requête non authentifiée). Permet de distinguer, par exemple, une clé absente d'une clé refusée.</summary>
    public Func<HttpContext, string> UnauthorizedMessage { get; set; } = _ => "Authentification requise.";

    /// <summary>Message d'un 403 sans permission identifiable (exigence d'autorisation autre qu'un rôle).</summary>
    public string ForbiddenMessage { get; set; } = "Accès refusé.";
}
