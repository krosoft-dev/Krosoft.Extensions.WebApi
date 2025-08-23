using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JetBrains.Annotations;
using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Abstractions.Models;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.Testing.Extensions;
using Krosoft.Extensions.WebApi.Identity.Extensions;
using Krosoft.Extensions.WebApi.Identity.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.WebApi.Identity.Tests.Services;

[TestClass]
[TestSubject(typeof(JwtTokenGenerator))]
public class JwtTokenGeneratorTests : BaseTest
{
    private readonly string _identifier = "user123";

    private readonly DateTime _now = new DateTime(2005, 05, 24);

    //TestInitialize
    private IJwtTokenGenerator _jwtTokenGenerator = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwtGenerator(configuration);

        var mock = new Mock<IOptions<JwtSettings>>();
        mock.Setup(x => x.Value)
            .Returns(new JwtSettings
            {
                SecurityKey = "KrosoftKdFzpfGOBEsKiw3rJrnpLaKLiSIIz",
                JwtTokenLifespan = 10
            });
        services.SwapTransient(_ => mock.Object);

        var mockDateTimeService = new Mock<IDateTimeService>();
        mockDateTimeService.Setup(x => x.Now).Returns(_now);
        services.AddTransient(_ => mockDateTimeService.Object);
    }

    [TestMethod]
    public void CreateToken_NoClaims()
    {
        var claims = new List<Claim>
        {
            new Claim("test", "unitaire")
        };
        var token = _jwtTokenGenerator.CreateToken(_identifier, claims);

        Check.That(token).IsNotNull();
        Check.ThatCode(() => ValidateJwtToken(token, claims)).DoesNotThrow();
    }

    [TestMethod]
    public void CreateToken_Null()
    {
        Check.ThatCode(() => _jwtTokenGenerator.CreateToken(null!, null!))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'identifier' est vide ou non renseignée.");
    }

    [TestMethod]
    public void CreateToken_NullClaims()
    {
        Check.ThatCode(() => _jwtTokenGenerator.CreateToken(_identifier, null!))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'claims' n'est pas renseignée.");
    }

    [TestMethod]
    public void CreateToken_Ok()
    {
        var claims = new List<Claim>
        {
            new Claim("test", "unitaire")
        };

        var token = _jwtTokenGenerator.CreateToken(_identifier, claims);

        Check.That(token).IsNotNull();
        Check.ThatCode(() => ValidateJwtToken(token, claims)).DoesNotThrow();
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _jwtTokenGenerator = serviceProvider.GetRequiredService<IJwtTokenGenerator>();
    }

    private void ValidateJwtToken(string token, IEnumerable<Claim> claims)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        Check.That(jsonToken).IsNotNull();
        Check.That(jsonToken!.ValidTo.ToUniversalTime()).IsAfter(_now.ToUniversalTime());
        Check.That(jsonToken.Claims.Select(x => x.Value)).Contains(claims.Select(x => x.Value));

        var claim = jsonToken.Claims.First(x => x.Type == ClaimTypes.Name);
        Check.That(claim.Value).IsEqualTo(_identifier);
    }
}