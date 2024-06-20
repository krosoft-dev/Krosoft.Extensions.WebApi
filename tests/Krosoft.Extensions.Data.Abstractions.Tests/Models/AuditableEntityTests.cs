using Krosoft.Extensions.Samples.Library.Models.Entities;

namespace Krosoft.Extensions.Data.Abstractions.Tests.Models;

[TestClass]
public class AuditableEntityTests
{
    [TestMethod]
    public void AuditableEntity_DefaultConstructor()
    {
        var entity = new Pays();

        Check.That(entity.CreateurDate).IsEqualTo(DateTime.MinValue);
        Check.That(entity.ModificateurDate).IsEqualTo(DateTime.MinValue);
    }

    [TestMethod]
    public void AuditableEntity_SetCreateurDate()
    {
        var entity = new Pays();
        var createurDate = DateTime.Now.AddDays(-1);

        entity.CreateurDate = createurDate;

        Check.That(entity.CreateurDate).IsEqualTo(createurDate);
    }

    [TestMethod]
    public void AuditableEntity_SetCreateurId()
    {
        var entity = new Pays();
        var createurId = "user123";

        entity.CreateurId = createurId;

        Check.That(entity.CreateurId).IsEqualTo(createurId);
    }

    [TestMethod]
    public void AuditableEntity_SetModificateurDate()
    {
        var entity = new Pays();
        var modificateurDate = DateTime.Now.AddDays(-1);

        entity.ModificateurDate = modificateurDate;

        Check.That(entity.ModificateurDate).IsEqualTo(modificateurDate);
    }

    [TestMethod]
    public void AuditableEntity_SetModificateurId()
    {
        var entity = new Pays();
        var modificateurId = "user456";

        entity.ModificateurId = modificateurId;

        Check.That(entity.ModificateurId).IsEqualTo(modificateurId);
    }
}