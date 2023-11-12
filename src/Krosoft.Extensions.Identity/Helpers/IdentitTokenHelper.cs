using Microsoft.IdentityModel.Tokens;
using Krosoft.Extensions.Identity.Abstractions.Models;

namespace Krosoft.Extensions.Identity.Helpers;

public static class IdentitTokenHelper
{
    public static TokenValidationParameters GetTokenValidationParameters(SigningCredentials signingCredentials, JwtSettings jwtSettings, bool validateLifetime) =>
        new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingCredentials.Key,
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = validateLifetime, // do not check for expiry date time.
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true
        };
}