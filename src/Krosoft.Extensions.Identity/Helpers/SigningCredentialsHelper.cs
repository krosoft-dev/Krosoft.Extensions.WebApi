using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Identity.Helpers;

public static class SigningCredentialsHelper
{
    private const string Algorithm = SecurityAlgorithms.HmacSha256Signature;

    public static SigningCredentials GetSigningCredentials(string securityKey)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(securityKey), securityKey);
        var key = Encoding.ASCII.GetBytes(securityKey);
        var symmetricSecurityKey = new SymmetricSecurityKey(key);

        return new SigningCredentials(symmetricSecurityKey, Algorithm);
    }

    public static SigningCredentials GetSigningCredentialsFromRsa(string pemFileContent)
    {
        using (var rsa = RSA.Create())
        {
            rsa.ImportFromPem(pemFileContent);

            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };
            return signingCredentials;
        }
    }

    public static bool CheckValidity(SecurityToken securityToken)
    {
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Contains(Algorithm, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return true;
    }
}