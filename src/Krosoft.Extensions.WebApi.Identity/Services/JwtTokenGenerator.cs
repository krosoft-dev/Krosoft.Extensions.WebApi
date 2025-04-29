using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Abstractions.Models;
using Krosoft.Extensions.WebApi.Identity.Helpers;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.WebApi.Identity.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeService _dateTimeService;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions, IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
        _jwtSettings = jwtOptions.Value;
    }

    public string CreateToken(string identifier,
                              IEnumerable<Claim> claims)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(identifier), identifier);
        Guard.IsNotNull(nameof(claims), claims);

        // "nbf" (Not Before) Claim - The "nbf" (not before) claim identifies the time before which the JWT MUST NOT be accepted for processing.
        var notBefore = _dateTimeService.Now;

        // "iat" (Issued At) Claim - The "iat" (issued at) claim identifies the time at which the JWT was issued.
        var issuedAt = notBefore.AddTicks(1);

        //Set the timespan the token will be valid for.
        var validFor = TimeSpan.FromMinutes(_jwtSettings.JwtTokenLifespan);

        //"exp" (Expiration Time) Claim - The "exp" (expiration time) claim identifies the expiration time on or after which the JWT MUST NOT be accepted for processing.
        var expiration = issuedAt.Add(validFor);

        var tokenClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, identifier),
            new Claim(JwtRegisteredClaimNames.Sub, identifier),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, issuedAt.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        tokenClaims.AddRange(claims);

        var signingCredentials = SigningCredentialsHelper.GetSigningCredentials(_jwtSettings.SecurityKey);

        var jwt = new JwtSecurityToken(_jwtSettings.Issuer,
                                       _jwtSettings.Audience,
                                       tokenClaims,
                                       notBefore.DateTime,
                                       expiration.DateTime,
                                       signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}