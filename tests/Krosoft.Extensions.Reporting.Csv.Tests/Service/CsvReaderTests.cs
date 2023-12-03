using System.Text;
using CsvHelper.TypeConversion;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Reporting.Csv.Extensions;
using Krosoft.Extensions.Reporting.Csv.Interfaces;
using Krosoft.Extensions.Reporting.Csv.Models;
using Krosoft.Extensions.Reporting.Csv.Tests.Models;
using Krosoft.Extensions.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Reporting.Csv.Tests.Service;

[TestClass]
public class CsvReaderTests : BaseTest
{
    private ICsvReadService _csvReadService = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCsv();
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _csvReadService = serviceProvider.GetRequiredService<ICsvReadService>();
    }

    [TestMethod]
    public async Task EncodingTest()
    {
        var lignesExport =
            new List<PrixCsvDto>
            {
                new PrixCsvDto
                {
                    FournisseurNom = "Bon Pied Bon Œil équipé",
                    DeviseCode = "EUR",
                    Prix = 10.5M,
                    ProduitNom = "TEST",
                    VarianteNom = "TEST"
                }
            };
        var exportFile = new CsvFileData<PrixCsvDto>(lignesExport, "test.csv", "");
        var export = Convert.ToBase64String(exportFile.ToBytes());
        await FileHelper.WriteBase64Async("test.csv", export, CancellationToken.None);

        var lignesNewExport = _csvReadService.GetRecordsFromPath<PrixCsvDto>("test.csv", Encoding.UTF8, "");
        Check.That(lignesNewExport.First().FournisseurNom).Equals("Bon Pied Bon Œil équipé");
    }

    [TestMethod]
    public async Task CultureTestFromBase64FrOk()
    {
        var csvFile = await File.ReadAllTextAsync("Files/test-fr.csv", CancellationToken.None);
        var lignes = _csvReadService.GetRecordsFromBase64<PrixCsvDto>(Convert.ToBase64String(Encoding.UTF8.GetBytes(csvFile)), Encoding.UTF8, "FR-fr").ToList();

        Check.That(lignes.First().FournisseurNom).Equals("Bon Pied Bon Œil équipé");
        Check.That(lignes.ElementAt(2).Prix).Equals(96.99);
        Check.That(lignes.First().Prix).Equals(10.5);
    }

    [TestMethod]
    public void CultureTestFromPathFrOk()
    {
        var lignes = _csvReadService.GetRecordsFromPath<PrixCsvDto>("Files/test-fr.csv", Encoding.UTF8, "FR-fr").ToList();

        Check.That(lignes.First().FournisseurNom).Equals("Bon Pied Bon Œil équipé");
        Check.That(lignes.ElementAt(2).Prix).Equals(96.99);
        Check.That(lignes.First().Prix).Equals(10.5);
    }

    [TestMethod]
    public async Task EncodingTestBase64Us()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var csvFile = await File.ReadAllTextAsync("Files/test-us.csv", CancellationToken.None);
        var lignes = _csvReadService.GetRecordsFromBase64<PrixCsvDto>(Convert.ToBase64String(Encoding.UTF8.GetBytes(csvFile)), Encoding.UTF8, "EN-us").ToList();

        Check.That(lignes.First().FournisseurNom).Equals("Bon Pied Bon Œil équipé");
        Check.That(lignes.ElementAt(2).Prix).Equals(9699);
        Check.That(lignes.First().Prix).Equals(10.5);
    }

    [TestMethod]
    public void EncodingTestFromPathUs()
    {
        var lignes = _csvReadService.GetRecordsFromPath<PrixCsvDto>("Files/test-us.csv", Encoding.UTF8, "EN-us").ToList();
        Check.That(lignes.First().FournisseurNom).Equals("Bon Pied Bon Œil équipé");
        Check.That(lignes.ElementAt(2).Prix).Equals(9699);
        Check.That(lignes.First().Prix).Equals(10.5);
    }

    [TestMethod]
    public async Task EncodingTestFrFromBase64NotOk()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var csvFile = await File.ReadAllTextAsync("Files/test-fr-faux.csv", CancellationToken.None);
        Check.ThatCode(() => { _csvReadService.GetRecordsFromBase64<PrixCsvDto>(Convert.ToBase64String(Encoding.UTF8.GetBytes(csvFile)), Encoding.UTF8, "FR-fr"); })
             .Throws<TypeConverterException>();
    }

    [TestMethod]
    public void EncodingTestFrNotOk()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Check.ThatCode(() => { _csvReadService.GetRecordsFromPath<PrixCsvDto>("Files/test-fr-faux.csv", Encoding.UTF8, "FR-fr"); })
             .Throws<TypeConverterException>();
    }
}