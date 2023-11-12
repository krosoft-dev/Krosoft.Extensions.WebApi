namespace Krosoft.Extensions.Identity.Abstractions.Models;

public class JwtSettings
{
    public string? SecurityKey { get; set; }

    /// <summary>
    /// "iss" (Issuer) Claim - The "iss" (issuer) claim identifies the principal that issued the JWT.
    /// </summary>
    public string? Issuer { get; set; }



    
    /// <summary>
    /// "aud" (Audience) Claim - The "aud" (audience) claim identifies the recipients that the JWT is intended for.
    /// </summary>
    public string? Audience { get; set; }

    /// <summary>
    /// Durée de vie du token de refresh (en minutes).
    /// </summary>
    public double RefreshTokenLifespan { get; set; }

    /// <summary>
    /// Durée de vie du token JWT (en minutes).
    /// </summary>
    public double JwtTokenLifespan { get; set; }
}
