using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Extensions;
using Krosoft.Extensions.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Identity.Tests.Services;

[TestClass]
public class ClaimsBuilderServiceTests : BaseTest
{
    private IClaimsBuilderService? _claimsBuilderService;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityEx();
    }

    [TestMethod]
    public void BuildAvecTenantIdTest()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id",

            TenantId = new Guid("00000000-1111-1111-1111-000000000001").ToString(),
            Nom = "Claim_Nom",
            Email = "Claim_Email",
            RoleId = "Claim_RoleId",
            RoleHomePage = "Claim_RoleHomePage",
            LangueId = "Claim_LangueId",
            LangueCode = "Claim_LangueCode"
        };
        var claims = _claimsBuilderService!.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims).HasSize(10);
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id", "nom", "email", "roleId", "roleIsInterne", "roleHomePage", "langueId", "langueCode", "droits", "tenantId");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id", "Claim_Nom", "Claim_Email", "Claim_RoleId", "False", "Claim_RoleHomePage", "Claim_LangueId", "Claim_LangueCode", "[]", "00000000-1111-1111-1111-000000000001");
    }

    [TestMethod]
    public void BuildNoLangueIdTest()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id",
            Nom = "Claim_Nom",
            Email = "Claim_Email",
            RoleId = "Claim_RoleId",
            RoleHomePage = "Claim_RoleHomePage",
            LangueCode = "Claim_LangueCode"
        };

        Check.ThatCode(() => { _claimsBuilderService!.Build(krosoftToken); })
             .Throws<KrosoftTechniqueException>()
             .WithMessage("La variable 'LangueId' est vide ou non renseignée.");
    }

    [TestMethod]
    public void BuildNoRoleIdTest()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id",
            Nom = "Claim_Nom",
            Email = "Claim_Email",
            RoleHomePage = "Claim_RoleHomePage",
            LangueId = "Claim_LangueId",
            LangueCode = "Claim_LangueCode"
        };

        Check.ThatCode(() => { _claimsBuilderService!.Build(krosoftToken); })
             .Throws<KrosoftTechniqueException>()
             .WithMessage("La variable 'RoleId' est vide ou non renseignée.");
    }

    [TestMethod]
    public void BuildNoTenantIdTest()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id",
            Nom = "Claim_Nom",
            Email = "Claim_Email",
            RoleId = "Claim_RoleId",
            RoleHomePage = "Claim_RoleHomePage",
            LangueId = "Claim_LangueId",
            LangueCode = "Claim_LangueCode"
        };

        var claims = _claimsBuilderService!.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims).HasSize(9);
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id", "nom", "email", "roleId", "roleIsInterne", "roleHomePage", "langueId", "langueCode", "droits");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id", "Claim_Nom", "Claim_Email", "Claim_RoleId", "False", "Claim_RoleHomePage", "Claim_LangueId", "Claim_LangueCode", "[]");
    }

    [TestMethod]
    public void BuildNullTest()
    {
        Check.ThatCode(() => { _claimsBuilderService!.Build(null); })
             .Throws<KrosoftTechniqueException>()
             .WithMessage("La variable 'krosoftToken' n'est pas renseignée.");
    }

    [TestMethod]
    public void BuildTest()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id",
            TenantId = "Claim_TenantId",
            Nom = "Claim_Nom",
            Email = "Claim_Email",
            RoleId = "Claim_RoleId",
            RoleHomePage = "Claim_RoleHomePage",
            LangueId = "Claim_LangueId",
            LangueCode = "Claim_LangueCode"
        };
        var claims = _claimsBuilderService!.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims).HasSize(10);
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id", "nom", "email", "roleId", "roleIsInterne", "roleHomePage", "langueId", "langueCode", "droits", "tenantId");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id", "Claim_Nom", "Claim_Email", "Claim_RoleId", "False", "Claim_RoleHomePage", "Claim_LangueId", "Claim_LangueCode", "[]", "Claim_TenantId");
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _claimsBuilderService = serviceProvider.GetRequiredService<IClaimsBuilderService>();
    }
}