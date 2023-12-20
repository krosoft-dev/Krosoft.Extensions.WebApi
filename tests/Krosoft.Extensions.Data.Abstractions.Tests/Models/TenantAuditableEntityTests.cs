using Krosoft.Extensions.Samples.Library.Models.Entities;
using NFluent;

namespace Krosoft.Extensions.Data.Abstractions.Tests.Models;

[TestClass]
public class TenantAuditableEntityTests
{
    [TestMethod]
    public void TenantAuditableEntity_DefaultConstructor()
    {
        var entity = new Utilisateur();

        Check.That(entity.CreateurDate).IsEqualTo(DateTime.MinValue);
        Check.That(entity.ModificateurDate).IsEqualTo(DateTime.MinValue);
    }

    [TestMethod]
    public void TenantAuditableEntity_SetCreateurDate ()
    {
        var entity = new Utilisateur();
        var createurDate = DateTime.Now.AddDays(-1);

        entity.CreateurDate = createurDate;

        Check.That(entity.CreateurDate).IsEqualTo(createurDate);
    }

    [TestMethod]
    public void TenantAuditableEntity_SetCreateurId ()
    {
        var entity = new Utilisateur();
        var createurId = "user123";

        entity.CreateurId = createurId;

        Check.That(entity.CreateurId).IsEqualTo(createurId);
    }

    [TestMethod]
    public void TenantAuditableEntity_SetModificateurDate ()
    {
        var entity = new Utilisateur();
        var modificateurDate = DateTime.Now.AddDays(-1);

        entity.ModificateurDate = modificateurDate;

        Check.That(entity.ModificateurDate).IsEqualTo(modificateurDate);
    }

    [TestMethod]
    public void TenantAuditableEntity_SetModificateurId ()
    {
        var entity = new Utilisateur();
        var modificateurId = "user456";

        entity.ModificateurId = modificateurId;

        Check.That(entity.ModificateurId).IsEqualTo(modificateurId);
    }



    [TestMethod]
    public void TenantAuditableEntity_SetTenantId ()
    {
        var entity = new Utilisateur();
        var tenantId = "user456";

        entity.TenantId = tenantId;

        Check.That(entity.TenantId).IsEqualTo(tenantId);
    }
}