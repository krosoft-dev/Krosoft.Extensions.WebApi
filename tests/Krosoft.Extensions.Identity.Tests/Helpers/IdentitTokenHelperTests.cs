using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Models;
using Krosoft.Extensions.Identity.Helpers;

namespace Krosoft.Extensions.Identity.Tests.Helpers;

[TestClass]
public class IdentitTokenHelperTests
{
    [TestMethod]
    public void GetTokenValidationParametersTest()
    {
        var jwtSettings = new JwtSettings
        {
            SecurityKey = "test"
        };
        var tokenValidationParameters = IdentitTokenHelper.GetTokenValidationParameters(SigningCredentialsHelper.GetSigningCredentials(jwtSettings.SecurityKey!), jwtSettings, false);

        Check.That(tokenValidationParameters).IsNotNull();
        Check.That(tokenValidationParameters.IssuerSigningKey).IsNotNull();
    }

    [TestMethod]
    public void JwtSettings_Null()
    {
        Check.ThatCode(() => IdentitTokenHelper.GetTokenValidationParameters(SigningCredentialsHelper.GetSigningCredentials("test"), null!, false))
             .Throws<KrosoftTechniqueException>()
             .WithMessage("La variable 'jwtSettings' n'est pas renseignée.");
    }

    [TestMethod]
    public void SigningCredentials_Null()
    {
        Check.ThatCode(() => IdentitTokenHelper.GetTokenValidationParameters(null!, null!, false))
             .Throws<KrosoftTechniqueException>()
             .WithMessage("La variable 'signingCredentials' n'est pas renseignée.");
    }
}