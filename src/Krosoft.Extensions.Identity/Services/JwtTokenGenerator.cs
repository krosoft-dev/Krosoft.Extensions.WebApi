using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Abstractions.Models;
using Krosoft.Extensions.Identity.Helpers;

namespace Krosoft.Extensions.Identity.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string CreateToken(string identifier,
                              IEnumerable<Claim> claims)
    {
        // "nbf" (Not Before) Claim - The "nbf" (not before) claim identifies the time before which the JWT MUST NOT be accepted for processing.
        var notBefore = DateTime.Now;

        // "iat" (Issued At) Claim - The "iat" (issued at) claim identifies the time at which the JWT was issued.
        var issuedAt = DateTime.Now;

        //Set the timespan the token will be valid for.
        var validFor = TimeSpan.FromMinutes(_jwtSettings.JwtTokenLifespan);

        //"exp" (Expiration Time) Claim - The "exp" (expiration time) claim identifies the expiration time on or after which the JWT MUST NOT be accepted for processing.
        var expiration = issuedAt.Add(validFor);

        var tokenClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, identifier),
            new Claim(JwtRegisteredClaimNames.Sub, identifier),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(issuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        tokenClaims.AddRange(claims);

        var signingCredentials = GetSigningCredentials();

        var jwt = new JwtSecurityToken(_jwtSettings.Issuer,
                                       _jwtSettings.Audience,
                                       tokenClaims,
                                       notBefore,
                                       expiration,
                                       signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public string? GetIdentifierFromToken(string accessToken)
    {
        var signingCredentials = GetSigningCredentials();
        var tokenValidationParameters = IdentitTokenHelper.GetTokenValidationParameters(signingCredentials, _jwtSettings, false);

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken,
                                                   tokenValidationParameters,
                                                   out var securityToken);

        SigningCredentialsHelper.CheckValidity(securityToken);

        return principal?.Identity?.Name;
    }

    private SigningCredentials GetSigningCredentials()
    {
        if (string.IsNullOrEmpty(_jwtSettings.SecurityKey))
        {
            throw new KrosoftTechniqueException($"'{nameof(_jwtSettings.SecurityKey)}' non définie.");
        }

        var signingCredentials = SigningCredentialsHelper.GetSigningCredentials(_jwtSettings.SecurityKey);
        return signingCredentials;
    }
}