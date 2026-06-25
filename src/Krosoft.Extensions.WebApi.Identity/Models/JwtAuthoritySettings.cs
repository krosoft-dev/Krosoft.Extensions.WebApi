namespace Krosoft.Extensions.WebApi.Identity.Models;

/// <summary>
/// Paramètres de validation des JWT émis par une autorité OIDC externe (Supabase, Auth0,
/// Keycloak, Entra ID...). Les clés de signature sont découvertes automatiquement via
/// <c>{Authority}/.well-known/openid-configuration</c> puis le endpoint JWKS — aucune clé
/// symétrique n'est stockée localement (contrairement à JwtSettings).
/// </summary>
public record JwtAuthoritySettings
{
    /// <summary>
    /// Autorité OIDC émettrice des JWT (ex. Supabase : <c>https://&lt;ref&gt;.supabase.co/auth/v1</c>).
    /// Sert à la fois d'<c>Authority</c> (découverte des clés) et d'issuer attendu (<c>iss</c>).
    /// </summary>
    public string? Authority { get; set; }

    /// <summary>
    /// "aud" (Audience) Claim - audience attendue dans le token (ex. Supabase : "authenticated").
    /// </summary>
    public string? Audience { get; set; }
}
