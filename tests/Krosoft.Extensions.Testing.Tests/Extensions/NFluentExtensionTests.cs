using System.Reflection;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Samples.Library.Factories;
using Krosoft.Extensions.Samples.Library.Models;
using Krosoft.Extensions.Testing.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Testing.Tests.Extensions;

[TestClass]
public class NFluentExtensionTests
{
    private const string ExportUtilisateursResourceName = "Krosoft.Extensions.Testing.Tests.Files.ExportUsers.csv";

    [TestMethod]
    public void ExtractingFromDataTableTest()
    {
        var users = new List<UtilisateurBasique>
        {
            UtilisateurFactory.CreateUtilisateur("nom1", "prenom1"),
            UtilisateurFactory.CreateUtilisateur("nom2", "prenom3")
        };

        var nomDataTable = "Utilisateurs";
        var datatable = users.ToDataTable(nomDataTable);
        Check.That(datatable).IsNotNull();
        Check.That(datatable.TableName).IsEqualTo(nomDataTable);
        Check.That(datatable.Rows).HasSize(2);
        Check.That(datatable.Extracting<UtilisateurBasique>(d => d.Nom!)).ContainsExactly("nom1", "nom2");
        Check.That(datatable.Extracting<UtilisateurBasique>(d => d.Prenom!)).ContainsExactly("prenom1", "prenom3");
    }

    [TestMethod]
    public void ExtractingTest()
    {
        var users = new List<UtilisateurBasique>
        {
            UtilisateurFactory.CreateUtilisateur("nom1", "prenom1"),
            UtilisateurFactory.CreateUtilisateur("nom2", "prenom3"),
            UtilisateurFactory.CreateUtilisateur("nom3", "prenom2")
        };

        Check.That(users).HasSize(3);
        Check.That(users.Select(c => c.Nom))
             .ContainsExactly("nom1", "nom2", "nom3");
        Check.That(users.Select(c => c.Prenom))
             .ContainsExactly("prenom1", "prenom3", "prenom2");
    }

    [TestMethod]
    public void IsEqualWithDeltaChainageTest()
    {
        Check.That(0.98m).IsEqualTo(0.98m).And.IsStrictlyLessThan(1);
        Check.That(0.98m).IsEqualWithDelta(0.98m, 0.01m).And.IsStrictlyLessThan(1);
    }

    [TestMethod]
    public void IsEqualWithDeltaTest()
    {
        Check.That(0.98m).IsEqualWithDelta(0.98m, 0.01m);
        Check.That(0.98m).IsEqualWithDelta(0.989m, 0.01m);
        Check.That(64.87m).IsEqualWithDelta(64.88m, 0.01m);
    }

    [TestMethod]
    public async Task IsFileEqualToEmbeddedFileTest()
    {
        var users = new List<UtilisateurBasique>
        {
            UtilisateurFactory.CreateUtilisateur("nom1", "prenom1"),
            UtilisateurFactory.CreateUtilisateur("nom2", "prenom3"),
            UtilisateurFactory.CreateUtilisateur("nom2", "prenom5")
        };

        var csv = users.ToCsv(";", u => u.DateModification);

        var ass = Assembly.GetExecutingAssembly();
        var folderPath = Path.GetDirectoryName(ass.Location)!;

        var fileName = $"ExportUsers_{DateTime.Now:yyyyMMdd-HHmmss}.csv";
        var filePath = Path.Combine(folderPath, fileName);

        await FileHelper.WriteAsync(filePath, csv);

        Check.That(filePath).IsFileEqualToEmbeddedFile(Assembly.GetExecutingAssembly(), ExportUtilisateursResourceName, 4);
    }
}