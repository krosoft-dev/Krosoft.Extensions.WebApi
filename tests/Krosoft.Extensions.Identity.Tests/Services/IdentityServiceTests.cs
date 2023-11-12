using System.Security.Claims;
using Krosoft.Extensions.Identity.Abstractions.Constantes;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Extensions;
using Krosoft.Extensions.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Krosoft.Extensions.Identity.Tests.Services;

[TestClass]
public class IdentityServiceTests : BaseTest
{
    private const string ProprietaireId = "test";
    private IIdentityService? _identityService;

    protected override void AddServices(ServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityEx();

        MockClaims(services);
    }

    private static void MockClaims(IServiceCollection services)
    {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(KrosoftClaimNames.Id, new Guid().ToString()),
            new Claim(KrosoftClaimNames.ProprietaireId, ProprietaireId),
            new Claim(KrosoftClaimNames.RoleIsInterne, true.ToString())
        }));

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var context = new DefaultHttpContext { User = claimsPrincipal };

        mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

        services.AddSingleton(mockHttpContextAccessor.Object);
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _identityService = serviceProvider.GetRequiredService<IIdentityService>();
    }

    [TestMethod]
    public void GetProprietaireIdTest()
    {
        var proprietaireId = _identityService!.GetProprietaireId();

        Check.That(proprietaireId).IsEqualTo(ProprietaireId);
    }

    [TestMethod]
    public void GetRoleIsInterneTest()
    {
        var roleIsInterne = _identityService!.GetRoleIsInterne();

        Check.That(roleIsInterne).IsTrue();
    }
}