using System.IdentityModel.Tokens.Jwt;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Abstractions.Models;
using Krosoft.Extensions.WebApi.Identity.Helpers;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.WebApi.Identity.Services;

public class JwtTokenValidator : IJwtTokenValidator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenValidator(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string? GetIdentifierFromToken(string accessToken)
    {
        var signingCredentials = SigningCredentialsHelper.GetSigningCredentials(_jwtSettings.SecurityKey);
        var tokenValidationParameters = IdentitTokenHelper.GetTokenValidationParameters(signingCredentials, _jwtSettings, false);

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken,
                                                   tokenValidationParameters,
                                                   out var securityToken);

        SigningCredentialsHelper.CheckValidity(securityToken);

        return principal?.Identity?.Name;
    }
}